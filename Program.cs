using System;
using System.IO;
using System.Text.Json; // JSON işleme için
using NAudio.Wave;
using Vosk;

namespace VoskMicDemo
{
    class Program
    {
        // Vosk'un döndürdüğü JSON'dan metni temizce ayıklamak için
        // yardımcı bir fonksiyon.
        static string GetTextFromJson(string json, string key)
        {
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    if (doc.RootElement.TryGetProperty(key, out JsonElement textElement))
                    {
                        return textElement.GetString() ?? string.Empty;
                    }
                }
            }
            catch (JsonException)
            {
                // Geçersiz veya boş JSON gelirse (örn. konuşma başlamadığında)
                return string.Empty;
            }
            return string.Empty;
        }

        static void Main(string[] args)
        {
            // 1. Vosk Modelini Yükle
            Vosk.Vosk.SetLogLevel(0); // Vosk loglarını açar (hata ayıklama için)
            string modelPath = "model-tr"; // Adım 0'da kopyaladığınız klasörün adı

            if (!Directory.Exists(modelPath))
            {
                Console.WriteLine($"HATA: Model klasörü bulunamadı!");
                Console.WriteLine($"Lütfen '{modelPath}' klasörünü programın çalıştığı dizine kopyalayın.");
                Console.WriteLine($"({Path.GetFullPath(".")})");
                Console.ReadKey();
                return;
            }

            // 'using' blokları, model ve tanıyıcıyı iş bittiğinde otomatik temizler
            using (Model model = new Model(modelPath))
            {
                // 2. Vosk Tanıyıcıyı Oluştur
                // ÖNEMLİ: Sample Rate (16000), NAudio formatıyla EŞLEŞMELİDİR.
                using (VoskRecognizer recognizer = new VoskRecognizer(model, 16000.0f))
                {
                    recognizer.SetMaxAlternatives(0); // Sadece en iyi sonucu göster
                    recognizer.SetWords(true); // Kelime zamanlamasını aç (isteğe bağlı)

                    // 3. NAudio ile Mikrofonu Ayarla
                    // 'using' ile mikrofonun da düzgün kapatılmasını sağlarız
                    using (WaveInEvent waveIn = new WaveInEvent())
                    {
                        // ÖNEMLİ: Vosk'un beklediği format (16kHz, 16-bit, Mono)
                        // Bu formatı NAudio'ya belirtmeliyiz.
                        waveIn.DeviceNumber = 0; // 0, varsayılan mikrofon demektir
                        waveIn.WaveFormat = new WaveFormat(16000, 16, 1);

                        // Mikrofondan ses verisi geldiğinde bu event (olay) tetiklenir
                        waveIn.DataAvailable += (sender, e) =>
                        {
                            // Sesi yakala ve anında Vosk'a besle
                            if (recognizer.AcceptWaveform(e.Buffer, e.BytesRecorded))
                            {
                                // AcceptWaveform 'true' dönerse, bir cümlenin bittiğini düşünür.
                                // 'recognizer.Result()' tam sonucu verir.
                                string resultText = GetTextFromJson(recognizer.Result(), "text");
                                if (!string.IsNullOrEmpty(resultText))
                                {
                                    Console.WriteLine($"[Cümle Bitti]: {resultText}");
                                }
                            }
                            else
                            {
                                // Henüz cümle bitmediyse, 'PartialResult' ile kısmi sonucu al
                                string partialText = GetTextFromJson(recognizer.PartialResult(), "partial");
                                if (!string.IsNullOrEmpty(partialText))
                                {
                                    // Kısmi sonucu aynı satıra yazdır (\r ile satır başına dön)
                                    // Bu, "canlı yazma" efekti verir.
                                    Console.Write($"[Konuşuluyor...]: {partialText}\r");
                                }
                            }
                        };

                        // 4. Dinlemeyi Başlat
                        waveIn.StartRecording();
                        Console.WriteLine("Dinliyorum... Konuşmaya başlayın.");
                        Console.WriteLine("(Durdurmak ve son sonucu görmek için Enter'a basın)");
                        Console.ReadLine();

                        // 5. Durdur ve Temizle
                        waveIn.StopRecording();

                        // Kayıt durduktan sonra, bellekte kalan son kısmı da işle
                        // ve 'FinalResult' ile sonucu al.
                        string finalText = GetTextFromJson(recognizer.FinalResult(), "text");
                        Console.WriteLine($"\n[Son Metin]: {finalText}");
                    }
                }
            }
        }
    }
}