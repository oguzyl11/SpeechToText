C# ile Çevrimdışı Speech-to-Text (Vosk & NAudio)
Bu proje, C# (.NET Core) kullanarak mikrofondan gelen sesi çevrimdışı (offline) olarak ve gerçek zamanlı bir şekilde metne dönüştüren bir konsol uygulamasıdır. Çekirdek motor olarak Vosk API'yi, ses yakalama için ise NAudio kütüphanesini kullanır.

🚀 Projenin Amacı
Bu uygulamanın temel amacı, internet bağlantısına veya bulut tabanlı servislere (Google Speech, Azure Speech vb.) ihtiyaç duymadan, kullanıcının cihazında (on-premise) konuşma tanıma yapmaktır. Bu sayede hem gecikme süresi düşürülür hem de veri gizliliği sağlanmış olur.

✨ Temel Özellikler
%100 Çevrimdışı Çalışma: İnternet bağlantısı gerektirmez. Tüm işlemler yerel makinede yapılır.

Gerçek Zamanlı Tanıma: Konuştuğunuz anda metni konsolda canlı olarak gösterir (PartialResult).

Yüksek Doğruluk: Vosk'un modern ve önceden eğitilmiş (Türkçe) dil modellerini kullanır.

Düşük Gecikme: Ses verisi dışarıya gönderilmediği için çok hızlı sonuç verir.

Kolay Kurulum: Sadece .NET Core ve gerekli dil modelini gerektirir.

🔧 Kullanılan Teknolojiler
C# (.NET 6.0+)

Vosk API (Konuşma tanıma motoru)

NAudio (Mikrofon erişimi ve ses yakalama)

Visual Studio 2022

⚙️ Kurulum ve Çalıştırma
Bu projeyi yerel makinenizde çalıştırmak için aşağıdaki adımları izleyin:

1. Projeyi Klonlayın:

Bash

git clone https://github.com/[KULLANICI_ADINIZ]/[PROJE_ADINIZ].git
cd [PROJE_ADINIZ]
2. Projeyi Visual Studio 2022 ile Açın: Proje dosyasını (.sln) açtığınızda, Visual Studio gerekli NuGet paketlerini (Vosk ve NAudio) otomatik olarak yükleyecektir.

3. Vosk Dil Modelini İndirin (En Önemli Adım): Bu proje, çalışmak için önceden eğitilmiş bir dil modeline ihtiyaç duyar.

Vosk Modelleri Resmi Sayfası'na gidin.

Listeden Türkçe modelini bulun (Örn: vosk-model-tr-0.3).

Modeli indirin ve ZIP dosyasından çıkarın.

Çıkardığınız klasörün adını (örn: vosk-model-tr-0.3) model-tr olarak değiştirin.

DİKKAT: İndirdiğiniz bu model-tr klasörünü, programın çalıştığı yere kopyalamanız gerekmektedir.

Projeyi Visual Studio'da Debug modunda çalıştırıyorsanız, klasörü bin/Debug/net6.0 (veya sizin .NET sürümünüz neyse) klasörünün içine yapıştırmalısınız.

Sonuçta dosya yapınız şöyle görünmelidir:

\bin\Debug\net6.0\
    ProjeAdiniz.exe
    Vosk.dll
    NAudio.dll
    \model-tr\   <-- (İçinde 'am', 'conf', 'graph' vb. klasörler olan model)
4. Projeyi Çalıştırın: Visual Studio üzerinden F5 tuşuna basarak veya aşağıdaki komutla projeyi başlatın:

Bash

dotnet run
Usage (Kullanım)
Uygulama başladığında konsolda "Dinliyorum... Konuşmaya başlayın." mesajını göreceksiniz.

Mikrofona konuşmaya başlayın.

Siz konuştukça, [Konuşuluyor...]: merhaba nasılsın şeklinde canlı döküm (partial result) göreceksiniz.

Konuşmayı bitirmek ve programı durdurmak için Enter tuşuna basın.

Enter'a bastığınızda, tüm konuşmanın tam metni [Son Metin]: merhaba nasılsın iyi misin şeklinde ekrana basılacaktır.

📄 Lisans
Bu proje MIT Lisansı ile lisanslanmıştır.
