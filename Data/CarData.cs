namespace AutoFocusASP.Data;

/// <summary>
/// Static catalogue of every car in the fleet, plus the brand groupings used by the
/// header, slide menu and home-page brand tiles. Text is stored in both English and
/// Bulgarian; the active language is chosen in the browser by toggling the
/// data-en / data-bg attributes rendered onto each element.
/// </summary>
public static class CarData
{
    public sealed record CarListing(string Url);

    public sealed record Car(
        string Slug,
        string Name,
        string Brand,
        string HeroImage,
        IReadOnlyList<string> Images,
        string DescriptionEn,
        string DescriptionBg,
        string SpecsEn,
        string SpecsBg,
        int SpecsMinHeight,
        IReadOnlyList<CarListing> Listings);

    /// <summary>Minimum height of the description card above the specifications.</summary>
    public const int DescriptionMinHeight = 156;

    public static readonly IReadOnlyList<Car> All = new Car[]
    {
        new(
            "lamborghini-huracan",
            "Lamborghini Huracan",
            "Lamborghini",
            "greenhuracanautofocus.png",
            new[]
            {
                "greenhuracan2.png",
                "huracan2.png",
                "huracan3.png",
                "huracan4.png",
                "huracan5.png",
            },
            "The Lamborghini Huracán is a high-performance, two-seat mid-engine supercar that was invented in 2013 and officially debuted in 2014",
            "Lamborghini Huracán е високопроизводителен двуместен суперавтомобил със средно разположен двигател, който е създаден през 2013 г. и официално дебютира през 2014 г.",
            "\nEngine: 5.2-liter naturally aspirated V10\nAcceleration (0–100 km/h): 2.9 to 3.4 seconds, varying by model\nTop Speed: Exceeds 325 km/h (202 mph)\nHorsepower: 580 to 640 hp (572 to 631 bhp) depending on the trim\nOfficial Lamborghini Price: $246,170 USD",
            "\nДвигател: 5,2-литров атмосферен V10\nУскорение (0–100 км/ч): 2,9 до 3,4 секунди, в зависимост от модела\nМаксимална скорост: Над 325 км/ч (202 мили/ч)\nМощност: 580 до 640 к.с. (572 до 631 bhp) в зависимост от нивото на оборудване\nОфициална цена на Lamborghini: $246 170 USD",
            220,
            new CarListing[]
            {
                new("https://www.mobile.bg/obiavi/avtomobili-dzhipove/lamborghini/huracan"),
                new("https://www.auto.bg/obiavi/avtomobili-dzhipove/lamborghini/huracan"),
                new("https://www.car24.bg/obiavi/lamborghini/huracan"),
                new("https://bazar.bg/obiavi/avtomobili/lamborghini/huracan"),
            }),

        new(
            "lamborghini-revuelto",
            "Lamborghini Revuelto",
            "Lamborghini",
            "blackrevueltoAF.png",
            new[]
            {
                "blackrevueltoAF.png",
                "revuelto2.png",
                "revuelto3.png",
                "revuelto4.png",
                "revuelto5.png",
            },
            "The Lamborghini Revuelto is a high-performance, two-seat mid-engine plug-in hybrid supercar that was officially unveiled in 2023",
            "Lamborghini Revuelto е високопроизводителен двуместен plug-in хибриден суперавтомобил със средно разположен двигател, който беше официално представен през 2023 г.",
            "\nEngine: 6.5-liter naturally aspirated V12 paired with 3 electric motors\nAcceleration (0–100 km/h): 2.4 to 2.5 seconds, varying by model\nTop Speed: Exceeds 345 to 350 km/h (214 to 217 mph)\nHorsepower: 1,001 to 1,065 hp (1,015 to 1,065 CV) depending on the trim\nOfficial Lamborghini Price: Starting at approximately $604,363 USD",
            "\nДвигател: 6,5-литров атмосферен V12, комбиниран с 3 електродвигателя\nУскорение (0–100 км/ч): 2,4 до 2,5 секунди, в зависимост от модела\nМаксимална скорост: Над 345 до 350 км/ч (214 до 217 мили/ч)\nМощност: 1 001 до 1 065 к.с. (1 015 до 1 065 CV) в зависимост от нивото на оборудване\nОфициална цена на Lamborghini: Начална цена от приблизително $604 363 USD",
            220,
            new CarListing[]
            {
                new("https://www.mobile.bg/obiavi/avtomobili-dzhipove/lamborghini/revuelto"),
                new("https://www.auto.bg/obiavi/avtomobili-dzhipove/lamborghini/revuelto"),
                new("https://www.car24.bg/obiavi/lamborghini/revuelto"),
                new("https://bazar.bg/obiavi/avtomobili/lamborghini/revuelto"),
            }),

        new(
            "lamborghini-urus",
            "Lamborghini Urus",
            "Lamborghini",
            "redurusAF.png",
            new[]
            {
                "redurusAF.png",
                "urus2.png",
                "urus3.png",
                "urus4.png",
                "urus5.png",
            },
            "The Lamborghini Urus is a high-performance, mid-size luxury Super SUV that was officially unveiled in 2017",
            "Lamborghini Urus е високопроизводителен средноразмерен луксозен Super SUV, който беше официално представен през 2017 г.",
            "\nEngine: 4.0-liter twin-turbocharged V8\nAcceleration (0–100 km/h): 3.3 to 3.6 seconds, varying by model\nTop Speed: Exceeds 305 to 312 km/h (190 to 194 mph)\nHorsepower: 641 to 799 hp depending on the trim\nOfficial Lamborghini Price: Starting at $230,000 to $260,000 USD",
            "\nДвигател: 4,0-литров twin-turbo V8\nУскорение (0–100 км/ч): 3,3 до 3,6 секунди, в зависимост от модела\nМаксимална скорост: Над 305 до 312 км/ч (190 до 194 мили/ч)\nМощност: 641 до 799 к.с. в зависимост от нивото на оборудване\nОфициална цена на Lamborghini: Начална цена от $230 000 до $260 000 USD",
            220,
            new CarListing[]
            {
                new("https://www.mobile.bg/obiavi/avtomobili-dzhipove/lamborghini/urus"),
                new("https://www.auto.bg/obiavi/avtomobili-dzhipove/lamborghini/urus"),
                new("https://www.car24.bg/obiavi/lamborghini/urus"),
                new("https://bazar.bg/obiavi/avtomobili/lamborghini/urus"),
            }),

        new(
            "ferrari-296gbt",
            "Ferrari 296 GTB",
            "Ferrari",
            "ferrari_296_GBT.png",
            new[]
            {
                "ferrari_296_GBT.png",
                "296fourferrari4.png",
                "296fourferrari3.png",
                "296fourferrari2.png",
                "296fourferrari1.png",
            },
            "The Ferrari 296 GTB is a high-performance, mid-engine luxury supercar that was officially unveiled in 2021.",
            "Ferrari 296 GTB е високопроизводителен луксозен суперавтомобил със средно разположен двигател, който беше официално представен през 2021 г.",
            "\nEngine: 3.0-liter twin-turbocharged V6\nAcceleration (0–100 km/h): 2.9 seconds\nTop Speed: Exceeds 330 km/h (205 mph)\nHorsepower: 819 hp\nOfficial Ferrari Price: Starting at $338,000 to $342,000 USD",
            "\nДвигател: 3,0-литров twin-turbo V6\nУскорение (0–100 км/ч): 2,9 секунди\nМаксимална скорост: Над 330 км/ч (205 мили/ч)\nМощност: 819 к.с.\nОфициална цена на Ferrari: Начална цена от $338 000 до $342 000 USD",
            220,
            new CarListing[]
            {
                new("https://www.mobile.bg/obiavi/avtomobili-dzhipove/ferrari/296gtb"),
                new("https://www.auto.bg/obiavi/avtomobili-dzhipove/ferrari/296"),
                new("https://www.car24.bg/obiavi/ferrari/296"),
                new("https://bazar.bg/obiavi/avtomobili/ferrari/296"),
            }),

        new(
            "ferrari-purosangue",
            "Ferrari Purosangue",
            "Ferrari",
            "Ferrari_Purosangue.png",
            new[]
            {
                "Ferrari_Purosangue.png",
                "yellowferrarifour1.png",
                "yellowferrarifour2.png",
                "yellowferrarifour3.png",
                "yellowferrarifour4.png",
            },
            "The Ferrari Purosangue is a high-performance, mid-front-mounted luxury sports car that was unveiled in 2022",
            "Ferrari Purosangue е високопроизводителен луксозен спортен автомобил със средно-предно разположен двигател, който беше представен през 2022 г.",
            "\nEngine: 6.5-liter naturally aspirated V12\nAcceleration (0–100 km/h): 3.3 seconds\nTop Speed: Exceeds 310 to 311 km/h\nHorsepower: 715 hp\nOfficial Ferrari Price: Starting at $393,000 to $400,000 USD",
            "\nДвигател: 6,5-литров атмосферен V12\nУскорение (0–100 км/ч): 3,3 секунди\nМаксимална скорост: Над 310 до 311 км/ч\nМощност: 715 к.с.\nОфициална цена на Ferrari: Начална цена от $393 000 до $400 000 USD",
            220,
            new CarListing[]
            {
                new("https://www.mobile.bg/obiavi/avtomobili-dzhipove/ferrari/purosangue"),
                new("https://www.auto.bg/obiavi/avtomobili-dzhipove/ferrari/purosangue"),
                new("https://www.car24.bg/obiavi/ferrari/purosangue"),
                new("https://bazar.bg/obiavi/avtomobili/ferrari/purosangue"),
            }),

        new(
            "ferrari-12cilindri",
            "Ferrari 12Cilindri",
            "Ferrari",
            "Ferrari_12Cilindri.png",
            new[]
            {
                "Ferrari_12Cilindri.png",
                "blueferrarifour1.png",
                "blueferrarifour2.png",
                "blueferrarifour3.png",
                "blueferrarifour4.png",
            },
            "The Ferrari 12Cilindri is a high-performance, front-mid-engined luxury grand tourer that was officially unveiled in 2024.",
            "Ferrari 12Cilindri е високопроизводителен луксозен гранд турър с предно-средно разположен двигател, който беше официално представен през 2024 г.",
            "\nEngine: 6.5-liter naturally aspirated V12\nAcceleration (0–100 km/h): 2.9 seconds\nTop Speed: Exceeds 340 km/h (211 mph)\nHorsepower: 819 hp (830 CV)\nOfficial Ferrari Price: Starting at $460,000 to $500,000 USD",
            "\nДвигател: 6,5-литров атмосферен V12\nУскорение (0–100 км/ч): 2,9 секунди\nМаксимална скорост: Над 340 км/ч (211 мили/ч)\nМощност: 819 к.с. (830 CV)\nОфициална цена на Ferrari: Начална цена от $460 000 до $500 000 USD",
            220,
            new CarListing[]
            {
                new("https://www.mobile.bg/obiavi/avtomobili-dzhipove/ferrari/12-cilindri"),
                new("https://www.auto.bg/obiavi/avtomobili-dzhipove/ferrari/12cilindri"),
                new("https://www.car24.bg/obiavi/ferrari/12cilindri"),
                new("https://bazar.bg/obiavi/avtomobili/ferrari/12cilindri"),
            }),

        new(
            "porsche-911-turbo-s",
            "Porsche 911 Turbo S",
            "Porsche",
            "cc1.png",
            new[]
            {
                "cc1.png",
                "blackporschefour1.png",
                "blackporschefour2.png",
                "blackporschefour3.png",
                "blackporschefour4.png",
            },
            "The Porsche 911 Turbo S is a high-performance, rear-engined luxury sports car that was officially unveiled in 1995.",
            "Porsche 911 Turbo S е високопроизводителен луксозен спортен автомобил със задно разположен двигател, който беше официално представен през 1995 г.",
            "\nEngine: 3.6-liter twin-turbocharged flat-6\nAcceleration (0–100 km/h): 2.5 seconds\nTop Speed: 330 km/h\nHorsepower: 640 to 711 hp, depending on model year\nOfficial Porsche Price: Starting at approximately $272,000 to $286,000 USD",
            "\nДвигател: 3,6-литров twin-turbo боксерен 6-цилиндров\nУскорение (0–100 км/ч): 2,5 секунди\nМаксимална скорост: 330 км/ч\nМощност: 640 до 711 к.с., в зависимост от моделната година\nОфициална цена на Porsche: Начална цена от приблизително $272 000 до $286 000 USD",
            220,
            new CarListing[]
            {
                new("https://www.mobile.bg/obiavi/avtomobili-dzhipove/porsche/911"),
                new("https://www.auto.bg/obiavi/avtomobili-dzhipove/porsche/911"),
                new("https://www.car24.bg/obiavi/porsche/911"),
                new("https://bazar.bg/obiavi/avtomobili/porsche/911"),
            }),

        new(
            "porsche-911-gt2-rs",
            "Porsche 911 GT2 RS",
            "Porsche",
            "cc5.png",
            new[]
            {
                "cc5.png",
                "redporschefour1.png",
                "redporschefour2.png",
                "redporschefour3.png",
                "redporschefour4.png",
            },
            "The Porsche 911 GT2 RS is a high-performance, rear-engined luxury supercar that was unveiled in 2010.",
            "Porsche 911 GT2 RS е високопроизводителен луксозен суперавтомобил със задно разположен двигател, който беше представен през 2010 г.",
            "\nEngine: 3.8-liter twin-turbocharged flat-6\nAcceleration (0–100 km/h): 2.8 seconds\nTop Speed: Exceeds 330 to 342 km/h\nHorsepower: 620 to 700 hp, depending on the track package\nOfficial Porsche Price: Starting at $245,000 to $293,000 USD",
            "\nДвигател: 3,8-литров twin-turbo боксерен 6-цилиндров\nУскорение (0–100 км/ч): 2,8 секунди\nМаксимална скорост: Над 330 до 342 км/ч\nМощност: 620 до 700 к.с., в зависимост от пистовия пакет\nОфициална цена на Porsche: Начална цена от $245 000 до $293 000 USD",
            220,
            new CarListing[]
            {
                new("https://www.mobile.bg/obiavi/avtomobili-dzhipove/porsche/911"),
                new("https://www.auto.bg/obiavi/avtomobili-dzhipove/porsche/911"),
                new("https://www.car24.bg/obiavi/porsche/911"),
                new("https://bazar.bg/obiavi/avtomobili/porsche/911"),
            }),

        new(
            "porsche-cayman-gt4-rs",
            "Porsche Cayman GT4 RS",
            "Porsche",
            "cc2.png",
            new[]
            {
                "cc2.png",
                "yellowporschefour1.png",
                "yellowporschefour2.png",
                "yellowporschefour3.png",
                "yellowporschefour5.png",
            },
            "The Porsche Cayman GT4 RS is a high-performance, mid-engined luxury supercar that was unveiled in 2021.",
            "Porsche Cayman GT4 RS е високопроизводителен луксозен суперавтомобил със средно разположен двигател, който беше представен през 2021 г.",
            "\nEngine: 4.0-liter naturally aspirated flat-6\nAcceleration (0–100 km/h): 3.4 seconds\nTop Speed: 315 km/h\nHorsepower: 493 hp\nOfficial Porsche Price: Starting at $141,000 to $161,000 USD",
            "\nДвигател: 4,0-литров атмосферен боксерен 6-цилиндров\nУскорение (0–100 км/ч): 3,4 секунди\nМаксимална скорост: 315 км/ч\nМощност: 493 к.с.\nОфициална цена на Porsche: Начална цена от $141 000 до $161 000 USD",
            220,
            new CarListing[]
            {
                new("https://www.mobile.bg/obiavi/avtomobili-dzhipove/porsche/cayman"),
                new("https://www.auto.bg/obiavi/avtomobili-dzhipove/porsche/cayman"),
                new("https://www.car24.bg/obiavi/porsche/cayman"),
                new("https://bazar.bg/obiavi/avtomobili/porsche/cayman"),
            }),

        new(
            "mclaren-model-1",
            "McLaren GT",
            "McLaren",
            "GT1.png",
            new[]
            {
                "GT1.png",
                "GT2.png",
                "GT3.png",
                "GT4.png",
                "GT5.png",
            },
            "The McLaren GT is a high-performance, two-seat mid-engine grand tourer that was invented in 2019 and officially debuted in 2019.",
            "McLaren GT е високодинамичен двуместен гранд турер (GT) с централно разположен двигател, създаден през 2019 г. и официално дебютирал през 2019 г.",
            "\nEngine: 4.0-liter twin-turbocharged V8\nAcceleration (0–100 km/h): 3.2 seconds\nTop Speed: Exceeds 326 km/h (203 mph)\nHorsepower: 612 hp (620 PS)\nOfficial McLaren Price: $210,000 USD",
            "\nДвигател: 4.0-литров V8 с двоен турбокомпресор\nУскорение (0–100 км/ч): 3.2 секунди\nМаксимална скорост: Надхвърля 326 км/ч (203 мили/ч)\nКонски сили: 612 к.с. (620 PS)\nОфициална цена на McLaren: $210 000 USD",
            220,
            new CarListing[]
            {
                new("https://www.mobile.bg/results?pubtype=1&marka=McLaren&model=GT"),
                new("https://www.auto.bg/obiavi/avtomobili-dzhipove/mclaren/gt"),
                new("https://www.car24.bg/obiavi/mclaren/gt"),
                new("https://www.autoscout24.bg/lst/mclaren/gt"),
            }),

        new(
            "mclaren-model-2",
            "McLaren 570S",
            "McLaren",
            "570S1.png",
            new[]
            {
                "570S1.png",
                "570S2.png",
                "570S3.png",
                "570S4.png",
                "570S5.png",
            },
            "The McLaren 570S is a high-performance, two-seat mid-engine sports car that was invented in 2015 and officially debuted in 2015.",
            "McLaren 570S е високодинамичен двуместен спортен автомобил с централно разположен двигател, създаден през 2015 г. и официално дебютирал през 2015 г.",
            "\nEngine: 3.8-liter twin-turbocharged V8\nAcceleration (0–100 km/h): 3.2 seconds\nTop Speed: Exceeds 328 km/h (204 mph)\nHorsepower: 562 hp (570 PS)\nOfficial McLaren Price: $188,600 USD",
            "\nДвигател: 3.8-литров V8 с двоен турбокомпресор\nУскорение (0–100 км/ч): 3.2 секунди\nМаксимална скорост: Надхвърля 328 км/ч (204 мили/ч)\nКонски сили: 562 к.с. (570 PS)\nОфициална цена на McLaren: $188 600 USD",
            220,
            new CarListing[]
            {
                new("https://www.mobile.bg/obiavi/avtomobili-dzhipove/mclaren/570s-coupe"),
                new("https://www.auto.bg/obiavi/avtomobili-dzhipove/mclaren/570s-coupe"),
                new("https://www.car24.bg/obiavi/mclaren/570s-coupe"),
                new("https://www.autoscout24.bg/lst/mclaren/570s/ot_%D1%83%D0%BF%D0%BE%D1%82%D1%80%D0%B5%D0%B1%D1%8F%D0%B2%D0%B0%D0%BD%D0%B8"),
            }),

        new(
            "mclaren-model-3",
            "McLaren 720S",
            "McLaren",
            "720S1.png",
            new[]
            {
                "720S1.png",
                "720S2.png",
                "720S3.png",
                "720S4.png",
                "720S5.png",
            },
            "The McLaren 720S is a high-performance, two-seat mid-engine supercar that was invented in 2017 and officially debuted in 2017.",
            "McLaren 720S е високодинамична двуместна суперкола с централно разположен двигател, създадена през 2017 г. и официално дебютирала през 2017 г.",
            "\nEngine: 4.0-liter twin-turbocharged V8\nAcceleration (0–100 km/h): 2.8 to 2.9 seconds\nTop Speed: Exceeds 341 km/h (212 mph)\nHorsepower: 710 hp (720 PS)\nOfficial McLaren Price: $284,745 USD",
            "\nДвигател: 4.0-литров V8 с двоен турбокомпресор\nУскорение (0–100 км/ч): 2.8 до 2.9 секунди\nМаксимална скорост: Надхвърля 341 км/ч (212 мили/ч)\nКонски сили: 710 к.с. (720 PS)\nОфициална цена на McLaren: $284 745 USD",
            220,
            new CarListing[]
            {
                new("https://www.mobile.bg/obiavi/avtomobili-dzhipove/mclaren/720-s"),
                new("https://bazar.bg/obiavi/avtomobili/mclaren/720-s"),
                new("https://www.car24.bg/obiavi/mclaren/720-s"),
                new("https://www.autoscout24.bg/lst/mclaren/720s/ot_%D1%83%D0%BF%D0%BE%D1%82%D1%80%D0%B5%D0%B1%D1%8F%D0%B2%D0%B0%D0%BD%D0%B8"),
            }),

        new(
            "bugatti-model-1",
            "Bugatti Chiron",
            "Bugatti",
            "buggati_chiron_1.png",
            new[]
            {
                "buggati_chiron_1.png",
                "chiron2.png",
                "chiron3.png",
                "chiron4.png",
                "chiron5.png",
            },
            "The Bugatti Chiron is a high-performance, two-seat mid-engine hypercar that was invented in 2015 and officially debuted in 2016.",
            "Bugatti Chiron е високодинамична двуместна хиперкола с централно разположен двигател, създадена през 2015 г. и официално дебютирала през 2016 г.",
            "\nEngine: 8.0-liter quad-turbocharged W16\nAcceleration (0–100 km/h): 2.4 seconds\nTop Speed: Exceeds 420 km/h (261 mph)\nHorsepower: 1,479 to 1,578 hp (1,500 to 1,600 PS) depending on the trim\nOfficial Bugatti Price: $2,998,000 USD",
            "\nДвигател: 8.0-литров W16 с четири турбокомпресора\nУскорение (0–100 км/ч): 2.4 секунди\nМаксимална скорост: Надхвърля 420 км/ч (261 мили/ч)\nКонски сили: 1479 до 1578 к.с. (1500 до 1600 PS) в зависимост от модификацията\nОфициална цена на Bugatti: $2 998 000 USD",
            220,
            new CarListing[]
            {
                new("https://www.autoscout24.bg/lst/bugatti/chiron"),
                new("https://www.classicdriver.com/en/cars/bugatti/chiron"),
                new("https://www.jamesedition.com/cars/bugatti/chiron"),
                new("https://www.carandclassic.com/list/12/chiron/"),
            }),

        new(
            "bugatti-model-2",
            "Bugatti Mistral",
            "Bugatti",
            "mistral1.png",
            new[]
            {
                "mistral1.png",
                "mistral2.png",
                "mistral3.png",
                "mistral4.png",
                "mistral5.png",
            },
            "The Bugatti Mistral is a high-performance, two-seat mid-engine hypercar that was invented in 2022 and officially debuted in 2024.",
            "Bugatti Mistral е високодинамична двуместна хиперкола с централно разположен двигател, създадена през 2022 г. и официално дебютирала през 2024 г.",
            "\nEngine: 8.0-liter quad-turbocharged W16\nAcceleration (0–100 km/h): 2.4 seconds\nTop Speed: Exceeds 420 km/h (261 mph)\nHorsepower: 1,578 hp (1,600 PS)\nOfficial Bugatti Price: $5,000,000 USD",
            "\nДвигател: 8.0-литров W16 с четири турбокомпресора\nУскорение (0–100 км/ч): 2.4 секунди\nМаксимална скорост: Надхвърля 420 км/ч (261 мили/ч)\nКонски сили: 1578 к.с. (1600 PS)\nОфициална цена на Bugatti: $5 000 000 USD",
            220,
            new CarListing[]
            {
                new("https://www.dupontregistry.com/cars-for-sale/bugatti/mistral"),
                new("https://www.classic.com/m/bugatti/mistral/"),
                new("https://www.classicdriver.com/en/cars/bugatti/mistral"),
                new("https://www.jamesedition.com/cars/bugatti/mistral"),
            }),

        new(
            "bugatti-model-3",
            "Bugatti Veyron",
            "Bugatti",
            "roadster1.png",
            new[]
            {
                "roadster1.png",
                "roadster2.png",
                "roadster3.png",
                "roadster4.png",
                "roadster5.png",
            },
            "The Bugatti Veyron is a high-performance, two-seat mid-engine supercar that was invented in 2001 and officially debuted in 2005.",
            "Bugatti Veyron е високодинамична двуместна суперкола с централно разположен двигател, създадена през 2001 г. и официално дебютирала през 2005 г.",
            "\nEngine: 8.0-liter quad-turbocharged W16\nAcceleration (0–100 km/h): 2.5 seconds\nTop Speed: Exceeds 407 km/h (253 mph)\nHorsepower: 1,001 to 1,183 hp (1,014 to 1,200 PS) depending on the trim\nOfficial Bugatti Price: $1,276,000 USD",
            "\nДвигател: 8.0-литров W16 с четири турбокомпресора\nУскорение (0–100 км/ч): 2.5 секунди\nМаксимална скорост: Надхвърля 407 км/ч (253 мили/ч)\nКонски сили: 1001 до 1183 к.с. (1014 до 1200 PS) в зависимост от модификацията\nОфициална цена на Bugatti: $1 276 000 USD",
            220,
            new CarListing[]
            {
                new("https://www.autoscout24.bg/lst/bugatti/veyron"),
                new("https://www.dupontregistry.com/used-bugatti-veyron-for-sale"),
                new("https://www.classic.com/m/bugatti/veyron/"),
                new("https://www.jamesedition.com/cars/bugatti/veyron"),
            }),

    };

    public static readonly IReadOnlyList<Car> Ferraris =
        All.Where(c => c.Brand == "Ferrari").ToList();

    public static readonly IReadOnlyList<Car> Lamborghinis =
        All.Where(c => c.Brand == "Lamborghini").ToList();

    public static readonly IReadOnlyList<Car> Porsches =
        All.Where(c => c.Brand == "Porsche").ToList();

    public static readonly IReadOnlyList<Car> McLarens =
        All.Where(c => c.Brand == "McLaren").ToList();

    public static readonly IReadOnlyList<Car> Bugattis =
        All.Where(c => c.Brand == "Bugatti").ToList();

    public static readonly IReadOnlyList<string> Brands = new[]
    {
        "Ferrari",
        "Lamborghini",
        "Porsche",
        "McLaren",
        "Bugatti"
    };

    public static readonly IReadOnlyDictionary<string, string> BrandLogos =
        new Dictionary<string, string>
        {
            ["Ferrari"] = "ferrari_logo_transparent_2.png",
            ["Lamborghini"] = "lamborghini_logo_transparent.png",
            ["Porsche"] = "porsche_logo_transparent.png",
            ["McLaren"] = "mclaren_logo_transparent_1.png",
            ["Bugatti"] = "bugatti_logo_1200x600_transparent.png"
        };

    public static Car? Find(string slug) =>
        All.FirstOrDefault(c => c.Slug == slug);

    public static IReadOnlyList<Car> ByBrand(string brand) =>
        All.Where(c => c.Brand == brand).ToList();
}

