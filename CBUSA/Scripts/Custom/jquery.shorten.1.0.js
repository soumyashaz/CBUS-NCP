/*
 * jQuery Shorten plugin 1.0.0
 *
 * Copyright (c) 2013 Viral Patel
 * http://viralpatel.net
 *
 * Dual licensed under the MIT license:
 *   http://www.opensource.org/licenses/mit-license.php
 */

(function ($) {
    $.fn.shorten = function (settings) {

        var config = {
            showChars: 100,
            ellipsesText: "...",
            moreText: "more",
            lessText: "less"
        };

        if (settings) {
            $.extend(config, settings);
        }

        $(document).off("click", '.morelink');

        $(document).on({
            click: function () {

                var $this = $(this);
                if ($this.hasClass('less')) {
                    $this.removeClass('less');
                    $this.html(config.moreText);
                    $this.css("background-color", "");
                } else {
                    $this.addClass('less');
                    $this.html(config.lessText);
                    $this.css("background-color", "#FBAF17");
                }
                $this.parent().prev().toggle();
                $this.prev().toggle();
                return false;
            }
        }, '.morelink');

        return this.each(function () {
            var $this = $(this);
            if ($this.hasClass("shortened")) return;

            $this.addClass("shortened");
            var content = $this.html();
            //console.log(content.length);

            var OnlyText = $(this).text();

            // console.log($(this).text());
            //if (content.length > config.showChars)
            //{
            //    console.log(content.trim());
            //}

            if (parseInt(OnlyText.trim().length) > parseInt("100")) {
                var c = content.substr(0, config.showChars);
                var h = content.substr(config.showChars, content.length - config.showChars);
                var html = c + '<span class="moreellipses">' + config.ellipsesText + ' </span><span class="morecontent"><span>' + h + '</span> <label href="#" class="morelink">' + config.moreText + '</label></span>';
                $this.html(html);
                $(".morecontent span").hide();
            }
        });

    };

})(jQuery);

