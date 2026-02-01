# LLM Kullanım Dokümantasyonu

> Bu dosyayı case boyunca kullandığınız LLM (ChatGPT, Claude, Copilot vb.) etkileşimlerini belgelemek için kullanın.
> Dürüst ve detaylı dokümantasyon beklenmektedir.

## Özet

| Bilgi | Değer |
|-------|-------|
| Toplam prompt sayısı | 5 |
| Kullanılan araçlar | Gemini |
| En çok yardım alınan konular |Folder Yapısı, Mimari Tasarım, Save/Load Logic, XML Docs & Naming Conventions |
| Tahmini LLM ile kazanılan süre | 4 saat |

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
> "I want to code myself on creating interfaces, classes, interactions etc. ... just give me plain classes and I will fill them." (Logic separation into Base Classes requested). Ayrıca Ludu Arts standartlarına (m_ prefix, XML summary vb.) uygun isimlendirmeler istendi.

**Alınan Cevap (Özet):**
> IInteractable arayüzü ve InteractionType enum'u oluşturuldu. Ardından BaseInteractable soyut sınıfı ve ondan türeyen Instant, Hold, Toggle base sınıfları hiyerarşik yapıda hazırlandı. Tüm kodlar istenilen XML dokümantasyon ve isimlendirme kurallarına göre düzenlendi.

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [x] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Gemini'nin önerdiği interface ve abstract class yapısı kullanıldı. Özellikle Ludu Arts standartlarına uygun `m_` prefix kullanımı ve XML summary gibi detayların hızlıca uygulanmasında LLM desteğinden yararlanıldı. Logic kısımları tarafımca uyarlandı.

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
> InteractionDetector sınıfının ana mantığı tarafımca yazıldı, sadece New Input System'in C# event syntax'ı için yardım alındı.

---

## Prompt 4: Save/Load System, Editor Tool ve Envanter Hatası

**Araç:** Gemini
**Tarih/Saat:** 2026-02-01

**Prompt:**
> Save/Load altyapısının kurulması, unique ID atayan editör aracı yazılması ve inventory'deki anahtarların yüklenmemesi sorununun çözümü.

**Alınan Cevap (Özet):**
> `ISaveable` arayüzü ve JSON serialization yapıldı. `SaveIDEditorWindow` aracı ile sahnedeki objelere ID atandı. Envanter yükleme sorunu için; `SimpleInventory` yükleme mantığı `Resources.LoadAll` kullanacak şekilde güncellendi ve anahtar kontrolü `ItemID` (string) üzerinden yapılarak referans hatası giderildi.

**Nasıl Kullandım:**
- [x] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim



---

## Prompt 5: C# Pattern Matching (Casting) Optimizasyonu

**Araç:** Gemini
**Tarih/Saat:** 2026-02-01

**Prompt:**
> Casting işlemleri için modern C# "Pattern Matching" (`if (obj is Type variable)`) yapısının kullanılması.

**Alınan Cevap (Özet):**
> `InteractionDetector` sınıfında `HoldInteractable` ve `BaseInteractable` dönüşümleri için klasik casting (`as` + `null check`) yerine pattern matching kullanıldı. Bu sayede hem tip kontrolü hem de değişken ataması tek satırda güvenli bir şekilde yapıldı.

**Nasıl Kullandım:**
- [x] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> InteractionDetector içinde interaksiyon tiplerini ayırt ederken ve highlight işlemlerinde kodun daha temiz ve performanslı olması için kullanıldı.
