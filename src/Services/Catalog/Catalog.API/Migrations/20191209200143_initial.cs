using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.API.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "catalog_brand_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "catalog_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "catalog_type_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "CatalogBrand",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Brand = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogBrand", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalogType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Type = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Catalog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    CatalogTypeId = table.Column<int>(nullable: false),
                    CatalogBrandId = table.Column<int>(nullable: false),
                    PictureName = table.Column<string>(nullable: true),
                    AvailableStock = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Catalog_CatalogBrand_CatalogBrandId",
                        column: x => x.CatalogBrandId,
                        principalTable: "CatalogBrand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Catalog_CatalogType_CatalogTypeId",
                        column: x => x.CatalogTypeId,
                        principalTable: "CatalogType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CatalogBrand",
                columns: new[] { "Id", "Brand" },
                values: new object[,]
                {
                    { 1, "Lenovo" },
                    { 2, "LG" },
                    { 3, "Samsung" },
                    { 4, "Huawei" }
                });

            migrationBuilder.InsertData(
                table: "CatalogType",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "Laptop" },
                    { 2, "Phone" }
                });

            migrationBuilder.InsertData(
                table: "Catalog",
                columns: new[] { "Id", "AvailableStock", "CatalogBrandId", "CatalogTypeId", "Description", "Name", "PictureName", "Price" },
                values: new object[,]
                {
                    { 1, 100, 1, 1, null, "Lenovo Thinkpad E560", "Laptop/l1.jpg", 5000000m },
                    { 2, 115, 2, 2, "اولین موردی که به چشم می خورد دوربین سه گانه (Galaxy A7 (2018 است که آن را برای عکاسی ایده ال می کند. سامسونگ گلسی ای 7 یکی از مدل هایی است که احتمالا به دلیل طراحی منحصر به فرد، نمایشگر و دوربین هایی که دارد نظرها را جلب خواهد کرد. از نظر قیمت هم می تواند با گوشی های دیگری چون POCO F1 ،Honor Play و Nokia 7 Plus رقابت کند که هر یک از آنها نکته مثبتی دارند برای مثال POCO F1 از قدرت باورنکردنی برخوردار بوده در حالی که Nokia 7 Plus از میان همه مواردی که گفته شد تجربه نرم افزاری بسیار خوبی را برای کاربران ارائه می دهد. این گوشی در میان موبایل های سامسونگ اولین و در برخی از بازارها بعد از گوشی هوآوی پی 20 پرو دومین موبایلی بوده که به دوربین سه گانه مجهز شده است. این دوربین ها شامل، سنسور 24 مگاپیکسلی با دیافراگم f/1.7 سنسور 8 مگاپیکسل به همراه لنزهای زاویه باز با دیافراگم f/2.4 و سنسور 5 مگاپیکسلی به همراه لنزهای فوکوس ثابت و دیافراگم f/2.2 هستند. نقطه برجسته در اینجا یک سنسور 8 مگاپیکسلی است به این دلیل که این دارای لنزهای فوق باز 13 میلی متری با زاویه دید 120 درجه است. در نگاه اول مشاهده می کنید که در ساخت آن، همان طراحی متداول سامسونگ به کار گرفته شده و مواد و طراحی و پرداختی که به کار رفته همانند موبایل سامسونگ گلکسی نوت 9 به نظر می رسد. این مدل همانند سامسونگ گلکسی ای 7 (2018) دارای فریم آلومنیومی با دو طرف منحنی نیست. پنل پشت شیشه ای آن دارای بازتاب های نوری است که با تغییر زاویه نور تغییر می کند. استفاده از شیشه در ساخت بدنه آن گرچه بسیار زیبا و ظریف به نظر می رسد اما به سرعت اثر انگشت را جذب می کند. زوایای آن به صورت منحنی طراحی شده تا راحت بتوان آن را در دست گرفت اما اندازه آن برای کار با یک دست کمی بزرگ است", "LG K9", "Phone/m2.jpg", 1500000m },
                    { 3, 115, 4, 2, "جدیدترین گوشی سری Y با مدل «Y7 Prime 2018 Dual SIM» از برند «هوآوی» با نمایشگری 5.9اینچی از نوع LCD در گروه فبلت‌ها دسته‌بندی می‌شود. نمایشگر 5.9اینچی و حافظه‌ی داخلی 32گیگابایتی از ویژگی‌های خوب این سری از گوشی‌هاست. همان‌طورکه‌ می‌دانید با گذشت زمان شرکت‌های مطرح و نام‌آشنای تولیدکننده‌ی کالای دیجیتال در تولید گوشی‌های موبایل با نمایشگر بزرگ، گوی سبقت را از یکدیگر ربوده‌اند. امروزه ابعاد صفحه‌نمایش به یکی از اولویت‌های افراد در خرید گوشی موبایل تبدیل شده است. همین امر به تولید محصولاتی با عنوان فبلت منجر شد که از نظر قدوقواره از گوشی‌های موبایل بزرگ‌تر و از تبلت‌ها کوچک‌تر هستند. وای ۷ پرایم ۲۰۱۸ به یک نمایشگر بزرگ 5.9اینچی با فناوری FullView مجهز شده است که وضوح تصویر 1440×720 پیکسل را به نمایش می‌‌کشد. با توجه به رزولوشنی که این نمایشگر دارد، تراکم پیکسلی 273 پیکسل در هر اینچ برای آن به‌دست می‌آید. سخت‌افزار این فبلت را تراشه‌‌ی Qualcomm MSM8937 Snapdragon 430، یک پردازنده‌ی هشت‌‌هسته‌ای و پردازنده‌‌ی گرافیکی Adreno 505 تشکیل می‌دهد. برای این دستگاه 3 گیگابایت حافظه‌ی رم و 32 گیگابایت حافظه‌ی داخلی در نظر گفته شده که توسط کارت حافظه‌ی microSD تا 256 گیگابایت قابل ‌افزایش است. شرکت هوآوی، این میزان ظرفیت و فضای حافظه‌ی داخلی را برای پاسخ به نیاز کاربرانی در نظر گرفته است که همواره فایل‌های صوتی و تصویری فراوانی برای ذخیره‌سازی در گوشی موبایل خود دارند. وجود دو سنسور با دقت 13 و 2 مگاپیکسل به‌عنوان دوربین‌های اصلی این گوشی موبایل، امکان ثبت تصاویر با رزولوشن بسیار زیاد را فراهم کرده‌اند؛ همچنین دوربین سلفی در وای ۷ پرایم ۲۰۱۸ به سنسوری 8مگاپیکسلی مجهز شده است که توانایی رقابت با بسیاری از محصولات موجود در بازار را دارد.", "Honor 4C", "Phone/m3.jpg", 1500000m },
                    { 4, 115, 3, 2, "پس از اینکه کاربران از گوشی‌های سری A «سامسونگ» با بدنه‌های باریک فلزی استقبال خوبی کردند، در سال 2017، این شرکت کره‌ای با معرفی دو گوشی J5 و J7 نشان داد که می‌خواهد بدنه‌ی فلزی را برای گوشی‌های میان‌رده‌اش هم به کار ببرد؛ اگرچه قبلا گوشی‌های J5 و J7 به بازار وارد شده بودند؛ اما نسل جدید این گوشی‌ها که ساخت سال 2017 هستند، علاوه‌بر تفاوتی که با نسل قبلی‌شان در مواد و متریال به‌کار‌رفته در ساخت بدنه‌شان دارند، تغییرات سخت‌افزاری هم داشته‌اند. نقطه قوت گوشی‌های سری J را می‌توان کیفیت ساخت بدنه، دوربین سلفی باکیفیت و سرعت اتصال آن‌ها به اینترنت دانست. گوشی سامسونگ «Galaxy J7 Pro» که در اینجا امکان خرید اینترنتی آن را دارید، 152.5 میلی‌متر طول، 74.8 میلی‌متر عرض و 7.9 میلی‌متر ضخامت دارد. بدنه‌ی آن از فلز و شیشه ساخته شده است و چیزی در حدود 181 گرم وزن دارد. صفحه‌نمایش 5.5اینچی آن که تراکمی برابر 401 پیکسل بر هر اینچ دارد، از نوع اولد است و فناوری سوپر امولد در آن استفاده شده است. این صفحه‌نمایش که قابلیت نمایش 16 میلیون رنگ دارد، از رزولوشن 1080×1920 پیکسل برخوردار است و قابلیت مالتی تاچ دارد و می‌تواند به‌ طور هم‌زمان 10 لمس را تشخیص دهد. دوربین اصلی آن، سنسور 13مگاپیکسلی دارد و به یک فلش LED مجهز شده است. به کمک این دوربین می‌توان با سرعت 30 فریم بر ثانیه و با رزولوشن 1080×1920 تصویربرداری کرد. دوربین سلفی یا دوربین دوم هم قابلیت فیلم‌برداری به‌صورت Full HD دارد و علاوه‌بر سنسور 13مگاپیکسلی به فلاش LED مجهز است. پردازنده‌ی این گوشی به تراشه‌ی اگزینوس 7870 از نوع 64بیتی مجهز است که 8 هسته دارد و می‌تواند اطلاعات را با فرکانس 1.6 گیگاهرتز پردازش کند. گلکسی J7 Pro حافظه رمی معادل 3 گیگابایت دارد و حافظه‌ی داخلی آن 64 گیگابایت است؛ البته به کمک شیار microSD که در بدنه‌ی آن جاسازی شده است، می‌توان ظرفیت ذخیره‌سازی را تا 256 گیگابایت افزایش داد. علاوه‌بر وای‌فای، این گوشی می‌تواند به شبکه‌های ارتباطی 3G و 4G هم دسترسی داشته باشد تا دسترسی به اطلاعات در فضای مجازی را برای صاحبش با سرعت زیادی میسر کند. در آخر باید گفت که سیستم‌عامل این گوشی اندروید نوقا (Nougat 7.0) است و باتری لیتیوم‌یون آن که 3600 میلی‌آمپرساعت ظرفیت دارد. محصولاتی که دارای گارانتی هماهنگ پردازش هستند از 18 ماه خدمات پس از فروش این شرکت بهره می‌برند، البته توجه داشته باشید که این گارانتی تنها تا تاریخ 1399/6/8 معتبر خواهد بود و این 18 ماه از زمان خرید محاسبه نمی‌شود.", "Samsung J7", "Phone/m4.jpg", 300000000m },
                    { 5, 115, 3, 2, "گوشی موبایل هوآوی مدل Nova 3i INE-LX1M ", "Samsung S5", "Phone/m5.jpg", 200000000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Catalog_CatalogBrandId",
                table: "Catalog",
                column: "CatalogBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Catalog_CatalogTypeId",
                table: "Catalog",
                column: "CatalogTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Catalog");

            migrationBuilder.DropTable(
                name: "CatalogBrand");

            migrationBuilder.DropTable(
                name: "CatalogType");

            migrationBuilder.DropSequence(
                name: "catalog_brand_hilo");

            migrationBuilder.DropSequence(
                name: "catalog_hilo");

            migrationBuilder.DropSequence(
                name: "catalog_type_hilo");
        }
    }
}
