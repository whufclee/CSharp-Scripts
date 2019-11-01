$(document).ready(function() {

    function printThis(el) {
        console.log($(this).text());
    }

    $('.text').val('');
    $('.search-input').val('');

    console.log($('.submenu a').first().text());

    $('#main').find('p').addClass('HAH');
    Sid(".my-selector").addClass("MYNEWCLASS")

    // disable normal right click behaviour
    $(document).on('contextmenu', function() {
        return false;
    })
    
    $(document).on('mousedown', function(event) {
        // event.stopPropagation();
        switch(event.which) {
            case 1:
                console.log('clicked left mouse button');
                break;
            case 2:
                console.log('clicked middle button');
                break;
            case 3:
                $('.hidden').removeClass('shown');
                if($(event.target).is('img')) {
                    alert('This is an image');
                    $('.saveimg, .newtab').addClass('shown');
                } else if ($(event.target).is('a')) {
                    $('.newtab').addClass('shown');
                    alert('This is a link');
                } else if ($(event.target).is('button')) {
                    $('.newtab').addClass('shown');
                    alert('This is a button');
                } else if ($(event.target).is('textbox')) {
                    $('.newtab').addClass('shown');
                    alert('This is text');
                } else {
                    alert('No idea WTF this is!');
                }

                $('#context').css({
                    top: event.pageY,
                    left: event.pageX
                });
                $('#context').fadeIn();
                return false;
        }
        $('#context').fadeOut();
    })

    $('[href="https://google.com"]').on('click', function(event) {
        console.log('linking to google');
    });

    var user = 'Lee';
    $('#username').html(user);
    var el = document.getElementById('text');

    $('#header-button').on('click', function() {
        alert("This is a work in progress!");
    });

    $('[data-trigger="dropdown"').on('mouseenter', function() {
        var submenu = $(this).parent().find('.submenu');
        // submenu.addClass('active');
        submenu.fadeIn(300);

        $('.profile-menu').on('mouseleave', function() {
            submenu.fadeOut(300);
        });
    });

    $('#prepend, #append, #replace').on('click', function (e) {
        var el = $(e.currentTarget);
        var action = el.attr('id');
        var content = $('.text').val();

        if(action=="prepend") {
            console.log("Prepending",content);
            $('#main').prepend(content);
        } else if(action=="append") {
            console.log("appending",content);
            $('#main').append(content);
        } else if(action=="replace") {
            console.log("replacing",content);
            $('#main').html(content);
        }
        $('.text').val('');
    })

    // $('p').each(function() {
    //     console.log($(this).text());
    // });

    $('p').each(printThis);

    $('p:contains("more")').html("This had more in it, it still does!");
    if ($(':contains("more")').is("p")) {
        console.log('More is in a paragraph');
    };
    
    $('input').focusout(function(){
        if($(this).val().indexOf('@') > -1 && $(this).val().indexOf('.') > -1) {
            $(".status").html("Valid email");
        } else {
            $(".status").html("Your email in invalid, try again please.");
        }
    })
})