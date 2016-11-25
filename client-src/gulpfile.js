'use strict';

var gulp = require('gulp'),
    watch = require('gulp-watch'),
    prefixer = require('gulp-autoprefixer'),
    uglify = require('gulp-uglify'),
    sass = require('gulp-sass'),
    rigger = require('gulp-rigger'),
    cleanCSS = require('gulp-clean-css'),
    imagemin = require('gulp-imagemin'),
    pngquant = require('imagemin-pngquant'),
    rimraf = require('rimraf'),
    browserSync = require('browser-sync'),
    reload = browserSync.reload,
    runSequence = require('run-sequence'),
    plumber = require('gulp-plumber'),
    jscs = require('gulp-jscs'),
    htmlhint = require('gulp-htmlhint'),
    spritesmith = require('gulp.spritesmith');


var path = {
    build: {
        html: '../EmotionalRatingBot/',
        js: '../EmotionalRatingBot/js/',
        css: '../EmotionalRatingBot/css/',
        img: '../EmotionalRatingBot/img/',
        spriteimg: 'src/img/',
        spritecss: 'src/style/components/'
    },
    src: {
        html: 'src/*.html',
        js: 'src/js/script.js',
        jsall: 'src/js/**/*.js',
        style: 'src/style/style.scss',
        img: 'src/img/**/*.*',
        sprite: 'src/img/sprite/*.*'
    },
    watch: {
        html: 'src/**/*.html',
        js: 'src/js/**/*.js',
        style: 'src/style/**/*.scss',
        img: 'src/img/**/*.*'
    }
};

var config = {
    server: {
        baseDir: "../EmotionalRatingBot/"
    },
    //tunnel: true,
    host: 'localhost',
    port: 9000,
    logPrefix: "Wanderer"
};

gulp.task('html:check', function () {
    gulp.src(path.src.html)
        .pipe(htmlhint())
        .pipe(htmlhint.reporter());
});

gulp.task('js:check', function () {
    gulp.src(path.src.jsall)
        .pipe(jscs())
        .pipe(jscs.reporter());
});

gulp.task('lint', [
    'html:check',
    'js:check'
]);

gulp.task('webserver', function () {
    browserSync(config);
});

gulp.task('sprite', function() {
    var spriteData =
        gulp.src(path.src.sprite) // путь, откуда берем картинки для спрайта
            .pipe(spritesmith({
                imgName: 'sprite.png',
                cssName: 'sprite.scss',
                cssFormat: 'scss',
                algorithm: 'binary-tree',
                imgPath: '../img/sprite.png'
            }));

    spriteData.img.pipe(gulp.dest(path.build.spriteimg)); // путь, куда сохраняем картинку
    spriteData.css.pipe(gulp.dest(path.build.spritecss)); // путь, куда сохраняем стили
});

gulp.task('html:build', function () {
    gulp.src(path.src.html)
        .pipe(plumber())
        .pipe(rigger())
        .pipe(gulp.dest(path.build.html))
        .pipe(reload({stream: true}));
});

gulp.task('js:build', function () {
    gulp.src(path.src.js)
        .pipe(plumber())
        .pipe(rigger())
        .pipe(uglify())
        .pipe(gulp.dest(path.build.js))
        .pipe(reload({stream: true}));
});

gulp.task('style:build', function () {
    gulp.src(path.src.style)
        .pipe(plumber())
        .pipe(sass({includePaths: ['src/style']}))
        // .pipe(sass({includePaths: ['src/style/']}).on('error', sass.logError))
        // .pipe(sass({
        //     includePaths: ['src/style/'],
        //     outputStyle: 'compressed'
        //     errLogToConsole: true
        // }))
        .pipe(prefixer())
        .pipe(cleanCSS())
        .pipe(gulp.dest(path.build.css))
        .pipe(reload({stream: true}));
});

gulp.task('image:build', function () {
    gulp.src(path.src.img)
        .pipe(imagemin({
            progressive: true,
            svgoPlugins: [{removeViewBox: false}],
            use: [pngquant()],
            interlaced: true
        }))
        .pipe(gulp.dest(path.build.img))
        .pipe(reload({stream: true}));
});

gulp.task('bundle', [
    'html:build',
    'js:build',
    'style:build',
    'image:build'
]);

gulp.task('build', function() {
  runSequence('sprite', 'bundle');
});

gulp.task('watch', function(){
    watch([path.watch.html], function(event, cb) {
        gulp.start('html:build');
    });
    watch([path.watch.style], function(event, cb) {
        gulp.start('style:build');
    });
    watch([path.watch.js], function(event, cb) {
        gulp.start('js:build');
    });
    watch([path.watch.img], function(event, cb) {
        gulp.start('image:build');
    });
});

gulp.task('reboot', [
    'webserver',
    'watch'
]);

gulp.task('default', [
    'build',
    'webserver',
    'watch'
]);
