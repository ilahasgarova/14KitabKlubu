using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using KitabKlubu.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Serverdə və ya domenə bağlayarkən yol problemi yaşamamaq üçün tam yol (absolute path) təyin edilir
var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "KitabKlubu.db");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.AdminUsers.Any())
    {
        db.AdminUsers.Add(new AdminUser { Username = "admin", PasswordHash = PasswordHelper.Hash("admin123") });
        db.SaveChanges();
    }

    if (!db.Articles.Any())
    {
        db.Articles.AddRange(
            new Article
            {
                Icon = "📖",
                Title = "Niyə Kağız Kitab Oxumaq Faydalıdır?",
                Summary = "Ekrandan uzaq, kağız üzərində oxumağın yaddaşa və diqqətə təsiri haqqında qısa bir baxış.",
                Content = "Tədqiqatlar göstərir ki, kağız üzərində oxuma zamanı beyin mətni daha dərin emal edir. " +
                          "Ekrandan oxuyarkən diqqətimiz tez-tez dağılır, gözlər yorulur və məzmun yaddaşda daha " +
                          "az qalır. Kağız kitab isə bizi bir növ 'rəqəmsal dincəlişə' aparır.",
                Date = "15.01.2026"
            },
            new Article
            {
                Icon = "✍️",
                Title = "İlk Kitab Klubu Görüşümüzün Təəssüratları",
                Summary = "Üzvlərimizin ilk görüşdə bölüşdüyü fikirlər, sevdiyi sitatlar və seçdiyimiz növbəti kitab.",
                Content = "İlk görüşümüz isti bir söhbət mühitində keçdi. Hər üzv öz sevdiyi sitatı bölüşdü, " +
                          "kitabın hansı hissəsinin onlara daha çox təsir etdiyini danışdı.",
                Date = "02.02.2026"
            },
            new Article
            {
                Icon = "📚",
                Title = "Azərbaycan Ədəbiyyatından 5 Tövsiyə",
                Summary = "Klassik və müasir Azərbaycan yazıçılarından hər kəsin oxumalı olduğu əsərlər siyahısı.",
                Content = "Azərbaycan ədəbiyyatı zəngin və çoxşaxəlidir. Klassiklərdən tutmuş müasir müəlliflərə " +
                          "qədər hər kəsə uyğun əsər tapmaq mümkündür.",
                Date = "10.03.2026"
            },
            new Article
            {
                Icon = "🕯️",
                Title = "Oxu Vərdişini Necə Formalaşdırmaq Olar?",
                Summary = "Gündəlik həyatda kitab oxumağa vaxt tapmaq üçün praktik məsləhətlər.",
                Content = "Oxu vərdişi yaratmaq üçün böyük məqsədlər qoymağa ehtiyac yoxdur — gündə 10-15 dəqiqə " +
                          "belə kifayətdir. Vacib olan davamlılıqdır.",
                Date = "28.03.2026"
            }
        );
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();