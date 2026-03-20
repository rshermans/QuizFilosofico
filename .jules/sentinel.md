## 2023-10-27 - Added CSRF Protection to Login
**Vulnerability:** The login form lacked Cross-Site Request Forgery (CSRF) protection.
**Learning:** ASP.NET Core MVC requires explicit `[ValidateAntiForgeryToken]` attribute on POST actions and `@Html.AntiForgeryToken()` in the corresponding view form.
**Prevention:** Always ensure forms that mutate state or authenticate users include CSRF tokens and the corresponding validation attribute on the controller action.
