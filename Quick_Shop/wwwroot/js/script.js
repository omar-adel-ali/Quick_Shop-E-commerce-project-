console.log('Running updated script.js - 2025-05-17');

document.addEventListener('DOMContentLoaded', () => {
    // Initialize Swiper for Hero Banner
    console.log('Initializing Swiper');
    new Swiper('.hero-slider', {
        loop: true,
        pagination: { el: '.swiper-pagination', clickable: true },
        navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
        },
        autoplay: { delay: 5000 },
    });

    // Theme Toggle
    console.log('Theme toggle initialized');
    const themeToggle = document.querySelector('.theme-toggle');
    if (themeToggle) {
        console.log('Theme toggle element found');
        themeToggle.addEventListener('click', () => {
            console.log('Theme toggle clicked');
            const currentTheme = document.documentElement.getAttribute('data-theme') || 'light';
            const newTheme = currentTheme === 'light' ? 'dark' : 'light';
            document.documentElement.setAttribute('data-theme', newTheme);
            themeToggle.textContent = newTheme === 'light' ? '🌙' : '☀️';
            localStorage.setItem('theme', newTheme);
        });

        const savedTheme = localStorage.getItem('theme') || 'light';
        document.documentElement.setAttribute('data-theme', savedTheme);
        themeToggle.textContent = savedTheme === 'light' ? '🌙' : '☀️';
    } else {
        console.log('Theme toggle element NOT found');
    }

    // Language Translations
    console.log('Initializing language translations');
    const translations = {
        en: {
            free_shipping: 'Free Shipping on Orders Over $50!',
            signup: 'Sign Up',
            login: 'Login',
            toggle_theme: '🌙',
            quick_shop: 'Quick Shop',
            home: 'Home',
            products: 'Products',
            cart: 'Cart',
            wishlist: 'Wishlist',
            profile: 'Profile',
            search_products: 'Search products...',
            mega_sale: 'Mega Sale',
            mega_sale_desc: 'Up to 50% off on all products!',
            shop_now: 'Shop Now',
            new_arrivals: 'New Arrivals',
            new_arrivals_desc: 'Discover the latest trends!',
            explore_now: 'Explore Now',
            special_offer: 'Special Offer',
            special_offer_desc: 'Buy one get one free!',
            shop_by_category: 'Shop by Category',
            fashion: 'Fashion',
            electronics: 'Electronics',
            books: 'Books',
            home_kitchen: 'Home & Kitchen',
            toys_games: 'Toys & Games',
            flash_sales: 'Flash Sales',
            hours: 'Hours',
            minutes: 'Minutes',
            seconds: 'Seconds',
            best_sellers: 'Best Sellers',
            seasonal_offer: 'Seasonal Offer',
            seasonal_offer_desc: 'Get 30% off on selected items this month!',
            customer_reviews: 'What Our Customers Say',
            review_1: '"Amazing products and fast delivery! Quick Shop is my go-to store."',
            review_2: '"Great deals and excellent customer service. Highly recommend!"',
            review_3: '"The variety of products is fantastic. I always find what I need."',
            about_us: 'About Us',
            about_us_desc: 'Quick Shop is your one-stop shop for all your needs. We offer a wide range of products at unbeatable prices.',
            quick_links: 'Quick Links',
            contact_us: 'Contact Us',
            phone: 'Phone: +1 234 567 890',
            address: 'Address: 123 Mega Street, Shop City',
            newsletter: 'Newsletter',
            enter_email: 'Enter your email',
            subscribe: 'Subscribe',
            categories: 'Categories',
            price_range: 'Price Range',
            apply_filters: 'Apply Filters',
            clear_filters: 'Clear Filters',
            sort_name: 'Sort by Name',
            sort_price_low: 'Price: Low to High',
            sort_price_high: 'Price: High to Low',
            shopping_cart: 'Shopping Cart',
            empty_cart: 'Your cart is empty.',
            product: 'Product',
            price: 'Price',
            quantity: 'Quantity',
            total: 'Total',
            action: 'Action',
            remove: 'Remove',
            clear_cart: 'Clear Cart',
            order_summary: 'Order Summary',
            subtotal: 'Subtotal',
            shipping: 'Shipping',
            proceed_checkout: 'Proceed to Checkout',
            empty_wishlist: 'Your wishlist is empty.',
            clear_wishlist: 'Clear Wishlist',
            billing_info: 'Billing Information',
            full_name: 'Full Name',
            email: 'Email',
            city: 'City',
            country: 'Country',
            select_country: 'Select Country',
            zip_code: 'Zip Code',
            place_order: 'Place Order',
            user_profile: 'User Profile',
            personal_info: 'Personal Information',
            order_history: 'Order History',
            no_orders: 'No orders yet.',
            edit_profile: 'Edit Profile',
            logout: 'Logout',
            password: 'Password',
            no_account: 'Don’t have an account? Sign Up',
            confirm_password: 'Confirm Password',
            have_account: 'Already have an account? Login',
            add_to_cart: 'Add to Cart',
            add_to_wishlist: 'Add to Wishlist',
            product_name: 'Product Name',
            product_desc: 'Product Description',
            no_products: 'No products match the selected filters.'
        },
        ar: {
            free_shipping: 'شحن مجاني للطلبات فوق 50 دولار!',
            signup: 'إنشاء حساب',
            login: 'تسجيل الدخول',
            toggle_theme: '🌙',
            quick_shop: 'التسوق السريع',
            home: 'الرئيسية',
            products: 'المنتجات',
            cart: 'السلة',
            wishlist: 'قائمة الرغبات',
            profile: 'الملف الشخصي',
            search_products: 'البحث عن المنتجات...',
            mega_sale: 'تخفيضات كبيرة',
            mega_sale_desc: 'خصم يصل إلى 50% على جميع المنتجات!',
            shop_now: 'تسوق الآن',
            new_arrivals: 'الوافدون الجدد',
            new_arrivals_desc: 'اكتشف أحدث الصيحات!',
            explore_now: 'استكشف الآن',
            special_offer: 'عرض خاص',
            special_offer_desc: 'اشترِ واحدة واحصل على واحدة مجانًا!',
            shop_by_category: 'تسوق حسب الفئة',
            fashion: 'الأزياء',
            electronics: 'الإلكترونيات',
            books: 'الكتب',
            home_kitchen: 'المنزل والمطبخ',
            toys_games: 'الألعاب',
            flash_sales: 'التخفيضات السريعة',
            hours: 'ساعات',
            minutes: 'دقائق',
            seconds: 'ثوانٍ',
            best_sellers: 'الأكثر مبيعًا',
            seasonal_offer: 'عرض موسمي',
            seasonal_offer_desc: 'احصل على خصم 30% على المنتجات المختارة هذا الشهر!',
            customer_reviews: 'آراء العملاء',
            review_1: '"منتجات رائعة وتوصيل سريع! التسوق السريع هو متجري المفضل."',
            review_2: '"عروض رائعة وخدمة عملاء ممتازة. أوصي به بشدة!"',
            review_3: '"تنوع المنتجات رائع. دائمًا أجد ما أحتاجه."',
            about_us: 'من نحن',
            about_us_desc: 'التسوق السريع هو متجرك الشامل لجميع احتياجاتك. نقدم مجموعة واسعة من المنتجات بأسعار لا تُضاهى.',
            quick_links: 'روابط سريعة',
            contact_us: 'اتصل بنا',
            phone: 'الهاتف: +1 234 567 890',
            address: 'العنوان: 123 شارع ميجا، مدينة التسوق',
            newsletter: 'النشرة الإخبارية',
            enter_email: 'أدخل بريدك الإلكتروني',
            subscribe: 'اشترك',
            categories: 'الفئات',
            price_range: 'نطاق السعر',
            apply_filters: 'تطبيق الفلاتر',
            clear_filters: 'مسح الفلاتر',
            sort_name: 'ترتيب حسب الاسم',
            sort_price_low: 'السعر: من الأقل إلى الأعلى',
            sort_price_high: 'السعر: من الأعلى إلى الأقل',
            shopping_cart: 'سلة التسوق',
            empty_cart: 'سلتك فارغة.',
            product: 'المنتج',
            price: 'السعر',
            quantity: 'الكمية',
            total: 'الإجمالي',
            action: 'الإجراء',
            remove: 'إزالة',
            clear_cart: 'مسح السلة',
            order_summary: 'ملخص الطلب',
            subtotal: 'المجموع الفرعي',
            shipping: 'الشحن',
            proceed_checkout: 'المتابعة إلى الدفع',
            empty_wishlist: 'قائمة رغباتك فارغة.',
            clear_wishlist: 'مسح قائمة الرغبات',
            billing_info: 'معلومات الفوترة',
            full_name: 'الاسم الكامل',
            email: 'البريد الإلكتروني',
            city: 'المدينة',
            country: 'الدولة',
            select_country: 'اختر الدولة',
            zip_code: 'الرمز البريدي',
            place_order: 'إتمام الطلب',
            user_profile: 'الملف الشخصي',
            personal_info: 'المعلومات الشخصية',
            order_history: 'تاريخ الطلبات',
            no_orders: 'لا توجد طلبات بعد.',
            edit_profile: 'تعديل الملف الشخصي',
            logout: 'تسجيل الخروج',
            password: 'كلمة المرور',
            no_account: 'ليس لديك حساب؟ اشترك',
            confirm_password: 'تأكيد كلمة المرور',
            have_account: 'لديك حساب بالفعل؟ تسجيل الدخول',
            add_to_cart: 'إضافة إلى السلة',
            add_to_wishlist: 'إضافة إلى قائمة الرغبات',
            product_name: 'اسم المنتج',
            product_desc: 'وصف المنتج',
            no_products: 'لا توجد منتجات مطابقة للفلاتر المختارة.'
        }
    };

    // Language Switcher
    console.log('Initializing language translations');
    function changeLanguage(lang) {
        console.log(`Changing language to: ${lang}`);
        document.documentElement.setAttribute('lang', lang);
        document.querySelectorAll('[data-translate]').forEach(elem => {
            const key = elem.getAttribute('data-translate');
            if (translations[lang][key]) {
                elem.textContent = translations[lang][key];
            }
        });
        document.querySelectorAll('[data-translate-placeholder]').forEach(elem => {
            const key = elem.getAttribute('data-translate-placeholder');
            if (translations[lang][key]) {
                elem.placeholder = translations[lang][key];
            }
        });
        localStorage.setItem('language', lang);
    }

    const savedLanguage = localStorage.getItem('language') || 'en';
    changeLanguage(savedLanguage);
    const langSelect = document.querySelector('select');
    if (langSelect) {
        console.log('Language select element found');
        langSelect.value = savedLanguage;
        langSelect.addEventListener('change', (e) => changeLanguage(e.target.value));
    } else {
        console.log('Language select element NOT found');
    }

    // Flash Sales Timer
    console.log('Initializing flash sales timer');
    const timer = {
        hours: document.getElementById('hours'),
        minutes: document.getElementById('minutes'),
        seconds: document.getElementById('seconds')
    };

    function startTimer() {
        let time = 24 * 60 * 60;
        setInterval(() => {
            const hrs = Math.floor(time / 3600);
            const mins = Math.floor((time % 3600) / 60);
            const secs = time % 60;
            if (timer.hours) timer.hours.textContent = String(hrs).padStart(2, '0');
            if (timer.minutes) timer.minutes.textContent = String(mins).padStart(2, '0');
            if (timer.seconds) timer.seconds.textContent = String(secs).padStart(2, '0');
            time = time <= 0 ? 24 * 60 * 60 : time - 1;
        }, 1000);
    }

    if (timer.hours) {
        console.log('Flash sales timer elements found');
        startTimer();
    } else {
        console.log('Flash sales timer elements NOT found');
    }
});