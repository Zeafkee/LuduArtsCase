# Interaction System - [Adınız Soyadınız]

> Ludu Arts Unity Developer Intern Case

## Proje Bilgileri

| Bilgi | Değer |
|-------|-------|
| Unity Versiyonu | 2022.3.x |
| Render Pipeline | URP |
| Case Süresi | Geliştirme aşaması |
| Tamamlanma Oranı | %40 |

---

## Kurulum

1. Repository'yi klonlayın.
2. Unity Hub'da projeyi açın.
3. `Assets/LuduInteraction/Scenes` klasörü altındaki test sahnesini oluşturup açın.

## Durum

Proje temel sistemleri ve oyuncu etkileşim algılayıcısı tamamlandı.
- **Core:** `IInteractable` ve base sınıflar hazır.
- **Player:** 
    - `PlayerController` (Movement & Cinemachine Look) tamamlandı.
    - `InteractionDetector` (Raycast tabanlı algılama, Collider caching sistemi ve New Input System entegrasyonu) tamamlandı.
- **Standartlar:** Ludu Arts kodlama standartlarına (XML Docs, prefix kullanımı, regionlar) uyum sağlandı.

## Sırada Ne Var?

- Concrete Interactables (`Door`, `Chest`, `Key`, `Switch`)
- UI Feedback Sistemi (Prompt & Progress Bar)
- Basit Envanter Sistemi

---