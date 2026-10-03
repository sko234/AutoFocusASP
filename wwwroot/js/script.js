/* =============================================================================
   AUTO FOCUS — client behaviour.

   The original build rebuilt the whole page from JavaScript on every hash
   change. This is a server-rendered MVC app, so everything here operates on
   markup that already exists: there is no template rendering, no innerHTML
   swapping and no client-side router.
   ========================================================================== */

(function () {
    'use strict';

    /* ---- Custom cursor -------------------------------------------------- */

    const cursor = document.querySelector('.cursor');

    if (cursor) {
        document.addEventListener('mousemove', function (e) {
            cursor.style.left = e.clientX + 'px';
            cursor.style.top = e.clientY + 'px';
        });

        document.addEventListener('mouseleave', function () { cursor.style.opacity = '0'; });
        document.addEventListener('mouseenter', function () { cursor.style.opacity = '1'; });

        document.querySelectorAll('a, button, .hover-link, .car-nav-btn, .burger-menu, .menu-link, .lang-toggle')
            .forEach(function (el) {
                el.addEventListener('mouseenter', function () { cursor.classList.add('hovered'); });
                el.addEventListener('mouseleave', function () { cursor.classList.remove('hovered'); });
            });
    }

    /* ---- Preloader ------------------------------------------------------ */

    const preloader = document.getElementById('preloader');
    const isHome = document.documentElement.getAttribute('data-page') === 'home';
    const INTRO_KEY = 'af_intro_seen';

    // The intro plays once per tab session, on the first page the tab loads.
    // Returning to the home page from a header or menu link is just navigation,
    // so it must not replay the intro or the click feels like a page reload.
    let introSeen = false;
    try {
        introSeen = sessionStorage.getItem(INTRO_KEY) === '1';
        if (!introSeen) { sessionStorage.setItem(INTRO_KEY, '1'); }
    } catch (e) {
        introSeen = false;
    }

    if (preloader) {
        if (isHome && !introSeen) {
            setTimeout(function () {
                preloader.style.opacity = '0';
                setTimeout(function () { preloader.style.display = 'none'; }, 1000);
            }, 1500);
        } else {
            preloader.style.display = 'none';
        }
    }

    /* ---- Language ------------------------------------------------------- */

    const LANG_KEY = 'af_lang';
    let currentLang = 'en';

    try {
        if (localStorage.getItem(LANG_KEY) === 'bg') { currentLang = 'bg'; }
    } catch (e) { /* private mode: fall back to English */ }

    function applyLang() {
        document.documentElement.setAttribute('lang', currentLang);
        document.body.classList.toggle('bg-active', currentLang === 'bg');

        document.querySelectorAll('[data-en]').forEach(function (el) {
            el.innerHTML = el.getAttribute('data-' + currentLang);
        });

        document.querySelectorAll('.lang-toggle, .lang-toggle-mobile').forEach(function (btn) {
            btn.textContent = currentLang === 'en' ? 'BG' : 'EN';
        });

        // Everything is swapped; nothing left to hide.
        document.documentElement.classList.remove('lang-pending');
    }

    function toggleLang() {
        currentLang = currentLang === 'en' ? 'bg' : 'en';
        try { localStorage.setItem(LANG_KEY, currentLang); } catch (e) { /* ignore */ }
        applyLang();
    }

    /* ---- Slide menu ----------------------------------------------------- */

    function setAuthAreaDark(isDark) {
        const authArea = document.getElementById('authArea');
        if (!authArea) { return; }

        authArea.querySelectorAll('.auth-link').forEach(function (el) {
            const isDashboard = el.getAttribute('href') === '/dashboard';
            const keepDashboardLight = isDark && isDashboard && window.matchMedia('(min-width: 769px)').matches;
            el.style.color = keepDashboardLight ? 'var(--color-text)' : (isDark ? '#050505' : 'var(--color-text)');
            if (el.tagName === 'BUTTON' || el.classList.contains('auth-link-boxed')) {
                el.style.borderColor = isDark ? '#050505' : 'var(--color-text)';
            }
        });

        const langToggle = authArea.querySelector('.lang-toggle');
        if (langToggle) {
            langToggle.style.color = isDark ? '#050505' : 'var(--color-text)';
            langToggle.style.borderColor = isDark ? '#050505' : 'var(--color-text)';
        }
    }

    function sizeSlideMenu() {
        if (!window.matchMedia('(min-width: 769px)').matches) { return; }

        const authArea = document.getElementById('authArea');
        if (!authArea) { return; }

        const dash = authArea.querySelector('.auth-link[href="/dashboard"]');
        let leftX;
        if (dash) {
            const out = authArea.querySelector('button.auth-link');
            if (!out) { return; }
            leftX = (dash.getBoundingClientRect().right + out.getBoundingClientRect().left) / 2;
        } else {
            const login = authArea.querySelector('.auth-link[href="/login"]');
            if (!login) { return; }
            leftX = login.getBoundingClientRect().left - 12;
        }

        slideMenu.style.setProperty('--menu-w', Math.max(200, document.documentElement.clientWidth - leftX) + 'px');
    }

    /* Desktop only: stretch the open-state burger into a wide X that reaches back
       to just before the end of the leftmost header link (DASHBOARD). */
    function sizeOpenBurger() {
        const authArea = document.getElementById('authArea');
        if (!authArea || !window.matchMedia('(min-width: 769px)').matches) { return; }

        let targetRight = null;
        authArea.querySelectorAll('.auth-link').forEach(function (el) {
            const rect = el.getBoundingClientRect();
            if (rect.width === 0) { return; }
            if (targetRight === null || rect.right < targetRight) {
                targetRight = rect.right;
            }
        });

        const burgerRect = burger.getBoundingClientRect();
        const width = targetRight === null
            ? burgerRect.width
            : Math.max(40, burgerRect.right - targetRight - 6);
        const height = 34;

        burger.style.setProperty('--x-len', Math.sqrt(width * width + height * height) + 'px');
        burger.style.setProperty('--x-angle', (Math.atan2(height, width) * 180 / Math.PI) + 'deg');
    }

    const burger = document.querySelector('.burger-menu');
    const slideMenu = document.getElementById('slideMenu');

    if (burger && slideMenu) {
        burger.addEventListener('click', function () {
            const willOpen = !burger.classList.contains('active');
            if (willOpen) {
                sizeSlideMenu();
                sizeOpenBurger();
            }
            const open = burger.classList.toggle('active');
            burger.setAttribute('aria-expanded', String(open));
            slideMenu.classList.toggle('active', open);
            slideMenu.setAttribute('aria-hidden', String(!open));
            setAuthAreaDark(open);
        });

        window.addEventListener('resize', function () {
            if (burger.classList.contains('active')) {
                sizeSlideMenu();
                sizeOpenBurger();
            }
        });

        slideMenu.querySelectorAll('a').forEach(function (a) {
            a.addEventListener('click', function () {
                burger.classList.remove('active');
                burger.setAttribute('aria-expanded', 'false');
                slideMenu.classList.remove('active');
                slideMenu.setAttribute('aria-hidden', 'true');
                setAuthAreaDark(false);
            });
        });
    }

    document.querySelectorAll('.lang-toggle, .lang-toggle-mobile').forEach(function (btn) {
        btn.addEventListener('click', toggleLang);
    });

    /* ---- Scroll-reveal animations --------------------------------------- */

    function observeReveals() {
        const observerOptions = { threshold: 0.1, rootMargin: '0px 0px -100px 0px' };

        if (!('IntersectionObserver' in window)) {
            // Without observer support, show everything rather than hide it.
            document.querySelectorAll('.reveal-text, .fade-up').forEach(function (el) {
                el.classList.add('active');
            });
            return;
        }

        const observer = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    entry.target.classList.add('active');
                    observer.unobserve(entry.target);
                }
            });
        }, observerOptions);

        document.querySelectorAll('.reveal-text, .fade-up').forEach(function (el) {
            observer.observe(el);
        });
    }

    /* ---- Header shadow -------------------------------------------------- */

    const header = document.getElementById('siteHeader');

    if (header) {
        window.addEventListener('scroll', function () {
            if (window.scrollY > 50) { header.classList.add('scrolled'); }
            else { header.classList.remove('scrolled'); }
        });
    }

    /* ---- Hero image grid ------------------------------------------------
       On desktop the 3x3 grid is positioned by script so it lines up exactly
       with the headline: same width, mirrored spacing above and below. */

    function positionHeroImageGrid() {
        const hero = document.querySelector('.hero');
        const title = document.querySelector('.hero-title');
        const scrollInd = document.querySelector('.scroll-indicator');
        const grid = document.getElementById('heroImageGrid');

        if (!hero || !title || !scrollInd || !grid) { return; }

        if (window.innerWidth <= 768) {
            grid.style.left = '';
            grid.style.top = '';
            grid.style.width = '';
            grid.style.height = '';
            grid.style.columnGap = '';
            return;
        }

        const heroRect = hero.getBoundingClientRect();
        const titleRect = title.getBoundingClientRect();
        const scrollRect = scrollInd.getBoundingClientRect();

        const width = titleRect.width;
        let colGap = 0.04 * width;
        let gridWidth = 5 * ((width - 2 * colGap) / 3) + 4 * colGap;
        const maxGridWidth = heroRect.width * 0.96;
        if (gridWidth > maxGridWidth) {
            const scale = maxGridWidth / gridWidth;
            gridWidth *= scale;
            colGap *= scale;
        }
        const left = titleRect.left - heroRect.left - (gridWidth - width) / 2;
        const titleTop = titleRect.top - heroRect.top;
        const titleBottom = titleRect.bottom - heroRect.top;
        const scrollTop = scrollRect.top - heroRect.top;

        // Space below the headline down to the scroll indicator
        const distDown = Math.max(scrollTop - titleBottom, 0);
        // Mirror the same distance above the headline
        const top = titleTop - distDown;
        const height = scrollTop - top;

        grid.style.left = left + 'px';
        grid.style.top = top + 'px';
        grid.style.width = gridWidth + 'px';
        grid.style.height = height + 'px';
        grid.style.columnGap = colGap + 'px';
    }

    /* ---- Car image slideshow ------------------------------------------- */

    function initCarSlider(root) {
        const img = root.querySelector('[data-car-slider-image]');
        const data = root.querySelector('script[type="application/json"]');
        if (!img || !data) { return; }

        let images;
        try {
            images = JSON.parse(data.textContent);
        } catch (e) {
            return;
        }
        if (!Array.isArray(images) || images.length === 0) { return; }

        let index = 0;
        let liked = root.getAttribute('data-carousel-liked') === 'true';

        const frame = root.querySelector('.car-image-frame');

        const setHeart = function (liked, animate) {
            if (!frame) { return; }

            const existing = frame.querySelector('.like-heart');
            if (existing) { existing.remove(); }
            if (!liked) { return; }

            const heart = document.createElement('span');
            heart.className = animate ? 'like-heart' : 'like-heart is-liked';
            heart.setAttribute('aria-hidden', 'true');
            heart.textContent = '❤️';
            frame.appendChild(heart);
        };

        const show = function () {
            img.src = images[index];
            setHeart(liked, false);
        };

        const step = function (delta) {
            index = (index + delta + images.length) % images.length;
            show();
        };

        const prev = root.querySelector('[data-car-slider-prev]');
        const next = root.querySelector('[data-car-slider-next]');

        if (prev) { prev.addEventListener('click', function () { step(-1); }); }
        if (next) { next.addEventListener('click', function () { step(1); }); }

        if (root.querySelector('[data-like-enabled="true"]')) {
            const token = document.querySelector('#image-like-token input[name="__RequestVerificationToken"]');

            if (frame && token) {
                img.addEventListener('dblclick', function () {
                    const formData = new FormData();
                    formData.append('__RequestVerificationToken', token.value);
                    formData.append('imageIndex', String(index));

                    fetch('/car/' + frame.dataset.carSlug + '/like', {
                        method: 'POST',
                        body: formData,
                        credentials: 'same-origin'
                    }).then(function (response) {
                        if (!response.ok) { throw new Error('Like request failed'); }
                        return response.json();
                    }).then(function (result) {
                        if (result.liked) {
                            liked = true;
                            setHeart(true, true);
                        } else {
                            liked = false;
                            setHeart(false, false);
                        }
                    }).catch(function () {
                        // Keep the current saved state visible when the request fails.
                    });
                });
            }
        }

        show();
    }

    document.querySelectorAll('[data-car-slider]').forEach(initCarSlider);

    /* ---- Comment replies ----------------------------------------------- */

    document.querySelectorAll('[data-reply-toggle]').forEach(function (button) {
        button.addEventListener('click', function () {
            var form = document.querySelector('[data-reply-form="' + button.dataset.replyToggle + '"]');
            if (form) { form.classList.toggle('is-open'); }
        });
    });

    /* ---- Data-href links -----------------------------------------------
       The hero tiles and the Fleet brand rectangles are the direct children of
       a grid, and the stylesheet addresses them with :nth-child. Wrapping them
       in <a> would change what :nth-child matches, so the markup is left
       exactly as it was and navigation is wired up here instead. */

    document.querySelectorAll('[data-href]').forEach(function (el) {
        el.setAttribute('role', 'link');
        if (!el.hasAttribute('tabindex')) { el.setAttribute('tabindex', '0'); }

        const go = function () { window.location.href = el.getAttribute('data-href'); };

        el.addEventListener('click', go);
        el.addEventListener('keydown', function (e) {
            if (e.key === 'Enter' || e.key === ' ') {
                e.preventDefault();
                go();
            }
        });
    });

    /* ---- Go ------------------------------------------------------------- */

    applyLang();
    observeReveals();
    positionHeroImageGrid();

    window.addEventListener('resize', positionHeroImageGrid);

    // The headline is measured, so re-run once the webfont lands or the grid
    // would be sized against the fallback font.
    if (document.fonts && document.fonts.ready) {
        document.fonts.ready.then(positionHeroImageGrid);
    }
})();
