# LLM Kullanım Dokümantasyonu

> Bu dosyayı case boyunca kullandığınız LLM (ChatGPT, Claude, Copilot vb.) etkileşimlerini belgelemek için kullanın.
> Dürüst ve detaylı dokümantasyon beklenmektedir.

## Özet

| Bilgi | Değer |
|-------|-------|
| Toplam prompt sayısı | 3 |
| Kullanılan araçlar | Gemini |
| En çok yardım alınan konular | Mimari Tasarım, Input System Syntax |
| Tahmini LLM ile kazanılan süre | 45 dk |

---

## Prompt 1: Proje Kurulumu ve Klasör Yapısı

**Araç:** Gemini
**Tarih/Saat:** 2026-01-31

**Prompt:**
> Implement the folder structures, namespaces, and the interface and base class structures specified in the case.

**Alınan Cevap (Özet):**
> Ludu Arts standartlarına uygun klasör hiyerarşisi oluşturuldu. Dokümantasyon dosyaları Docs/ altına taşındı.

**Nasıl Kullandım:**
- [x] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Projenin dosya sistemini manuel oluşturmak yerine otomatize ederek zaman kazanıldı.

---

## Prompt 2: Core Interaction Mimarisinin Kurulması

**Araç:** Gemini
**Tarih/Saat:** 2026-01-31

**Prompt:**
> "I want to code myself on creating interfaces, classes, interactions etc. ... just give me plain classes and I will fill them." (Logic separation into Base Classes requested)

**Alınan Cevap (Özet):**
> IInteractable arayüzü ve InteractionType enum'u oluşturuldu. Ardından BaseInteractable soyut sınıfı ve ondan türeyen Instant, Hold, Toggle base sınıfları hiyerarşik yapıda hazırlandı.

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [x] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Gemini'nin önerdiği interface ve abstract class yapısı kullanıldı ancak BaseInteractable üzerindeki logic (debug logları vb.) tarafımca değiştirilerek projeye uyarlandı.

---

## Prompt 3: New Input System Entegrasyonu (Syntax)

**Araç:** Gemini
**Tarih/Saat:** 2026-01-31

**Prompt:**
> New Input System üzerinde InputActionReference kullanımı ve started/canceled eventlerine abone olma kuralı/syntax'ı.

**Alınan Cevap (Özet):**
> InputActionReference üzerinden action'a erişim, Enable/Disable gerekliliği ve += operatörü ile event aboneliği syntax'ı sağlandı.

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [x] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> InteractionDetector sınıfının ana mantığı (Raycast, Caching vb.) tarafımca yazıldı, sadece New Input System'in C# tarafındaki özel event syntax'ı için yardım alındı.

---