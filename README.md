# Farm Idle 🌾

Unity (2D URP) ile geliştirilmiş çiftlik yönetimi, kaynak toplama, alan kilidi açma ve zindan/savaş mekaniklerine sahip **2D Top-Down Idle / Arcade RPG** oyunu.

🎮 **Oyunu Tarayıcıda veya Android'de Oyna (Itch.io):**  
👉 **[https://eeneskpln.itch.io/farm-idle](https://eeneskpln.itch.io/farm-idle)**

---

## 🛠️ Öne Çıkan Sistemler ve Mimari (`Assets/Scripts/`)

* **Ekonomi ve Kaynak Yönetimi (`EconomyManager.cs`, `MoneyUI.cs`, `WoodUI.cs`):** Altın ve odun kaynaklarının Singleton tabanlı yönetimi ve dinamik UI senkronizasyonu.
* **Kademeli Alan Kilidi Açma (`KilitAcmaAlani.cs`):** Oyuncunun kaynak ödeyerek yeni harita bölgelerini, ev geliştirmelerini, hayvan çiftliklerini ve mağara girişini kademeli olarak açması (kısmi ödeme kaydı destekli).
* **Çapraz Platform & WebGL Kayıt Sistemi (`SaveSystem.cs`, `Assets/Plugins/WebGL/WebGLStorage.jslib`):** Masaüstü, Android (`PlayerPrefs`) ve WebGL / iOS Safari (`localStorage`, `Cookie` ve `IndexedDB` senkronizasyonu) için kalıcı otomatik kayıt sistemi.
* **Yapay Zeka ve Dövüş Sistemi (`EnemyAI.cs`, `MantarAI.cs`, `FareAI.cs`, `PlayerCombat.cs`, `Ok.cs`):** Durum makinesi (State Machine) tabanlı düşman davranışları, dash saldırıları, alan hasarı (AoE) ve menzilli okçu dövüş mekanikleri.
* **Üretim ve Çiftlik Döngüsü (`TarlaAlani.cs`, `AgacKesme.cs`, `HayvanAI.cs`, `BalikTutmaAlani.cs`, `SatisAlani.cs`):** Tohum ekme/sulama, ağaç kesme, balık tutma, hayvan besleme ve envanter satış döngüsü.

---

## 📦 Lisans Notu (Görsel ve Ses Assetleri)

Projede kullanılan ücretli 2D görsel paketleri (`Assets/Sprites/`, `Assets/Tilemap/`), ses dosyaları ve üçüncü parti paketler **telif ve lisans kuralları gereği** bu herkese açık (public) repoya dahil edilmemiştir (`.gitignore` ile hariç tutulmuştur). Kaynak kodların tamamı [`Assets/Scripts/`](Assets/Scripts) klasöründe incelenebilir.
