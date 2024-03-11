# AGSR Test

**Вакансия:** https://rabota.by/vacancy/94143372

**Резюме HH:** https://rabota.by/applicant/resumes/view?resume=2a6e5e68ff0cc53ca20039ed1f556a55505253

---

**Запуск:**

Build solution:
```
dotnet build
```
В корне solution:
```
build compose up --build
```
---

Swagger доступен:

http://localhost:8080/swagger/index.html

---

***Не успел разобраться и реализовать:***

1. Возвращать кастомный messege с ошибкой.
2. Search:
   - api не работает, если передаем дату как год (например ?date=ge2024)
   - 3 префикса для поиска не реализованны (sa, eb, ap)
3. Покрыть все тестами.
4. Возможно еще что то...
