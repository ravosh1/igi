
var chatHub = $.connection.chatHub;

function InitiateEmoji_1(elem) {
    //alert("ffff");
    //$('.msg_box').each(function () {
    var ctrId = $(elem).attr('id');
    //if (!$(elem).hasClass('emojiadded')) {
            $("#input_" + ctrId + "").emojioneArea({
                autocomplete: false,
                events: {
                    keyup: function (editor, event) {
                        $("#input_" + ctrId + "").val(editor.html());                      
                    }
                    ,
                    keypress: function (editor, e) {


                        if (e.keyCode == 13) {
                            e.preventDefault();
                            var msg = editor.html();

                            $(editor).html('');
                            if (msg != null && msg.trim().length > 0) {
                                var userId = $("#input_" + ctrId + "").parents('#messageBoxCntainer .msg_box').attr("data-userid");

                                //var imgurl = $("#thumbnil_" + ctrId).attr('src');

                                

                                chatHub.server.sendPrivateMessage(userId, msg);
                            }
                        }
                        else
                        {
                            var userId = $("#input_" + ctrId + "").parents('#messageBoxCntainer .msg_box').attr("data-userid");
                            chatHub.server.userTyping(userId, msg);
                        }

                    }
                }

            });
        //}
        //else
        //{
        //    $("#input_" + ctrId + "").emojioneArea({ autocomplete: false });
        //}
    //});

}

function InitiateEmoji() {


    $(".msg_box .emojionearea").remove();
    $('.msg_box').each(function () {
        var ctrId = $(this).attr('id');
        //if (!$(this).hasClass('emojiadded')) {
            $("#input_" + ctrId + "").emojioneArea({
                autocomplete: false,
                events: {
                    keyup: function (editor, event) {
                        $("#input_" + ctrId + "").val(editor.html());
                    },
                    keypress: function (editor, e) {

                        if (e.keyCode == 13) {
                            e.preventDefault();
                            var msg = editor.html();

                            $(editor).html('');
                            if (msg != null && msg.trim().length > 0) {
                                var userId = $("#input_" + ctrId + "").parents('#messageBoxCntainer .msg_box').attr("data-userid");

                                var imgurl = $("#thumbnil_" + ctrId).attr('src');
                                alert(imgurl);

                                //$("#input_" + ctrId).val(imgurl);

                                chatHub.server.sendPrivateMessage(userId, msg);
                            }
                        }
                        else {
                            var userId = $("#input_" + ctrId + "").parents('#messageBoxCntainer .msg_box').attr("data-userid");
                            chatHub.server.userTyping(userId, msg);
                        }

                    }
                }

            });
        //}
        //else {
        //    $("#input_" + ctrId + "").emojioneArea({ autocomplete: false });
        //}
    });
}

$(function () {
   //Chat Hub------------------
    //setScreen(false);

    // Declare a proxy to reference the hub. 
    registerClientMethods(chatHub);
    // Start Hub
    $.connection.hub.start().done(function () {
        registerEvents(chatHub);
    });

    Array.remove = function (array, from, to) {
        var rest = array.slice((to || from) + 1 || array.length);
        array.length = from < 0 ? array.length + from : from;
        return array.push.apply(array, rest);
    };
    //this variable represents the total number of popups can be displayed according to the viewport width
    var total_popups = 0;
    //arrays of popups ids
    var popups = [];


    function registerEvents(chatHub) {
        var name = $('#lusername').val();
        var uid = $('#luserid').val();
        if (name != null && uid != null && name != undefined && uid != undefined && name.length > 0 && uid.length > 0) {
            chatHub.server.connect(name, uid);
        }
        else
        {
            // window.location = '@Url.Action("Index","Home")';
        }
    }

    function registerClientMethods(chatHub) {

        // Calls when user successfully logged in
        chatHub.client.onConnected = function (id, userName, allUsers, messages) {
            //setScreen(true);          
            $('#luserconid').val(id);
            $("#divusers").html('');
           
            // Add All Users
            for (i = 0; i < allUsers.length; i++) {
                console.log('connected:' + allUsers[i].name + ":" + allUsers[i].avator);
                AddUser(chatHub, allUsers[i].ConnectionId, allUsers[i].name, allUsers[i].userid, allUsers[i].avator, allUsers[i].IsOnline);
            }
            console.log('connected:' + userName);
        }
        // On New User Connected
        chatHub.client.onNewUserConnected = function (id, name, uid, dp)
        {          
            makeUseroffon(chatHub, id, name, uid, dp, "Y");
        }

        chatHub.client.newOfflineUser = function (id, name, uid, dp)
        {           
            makeUseroffon(chatHub, id, name, uid, dp, "N");
            console.log('newofflineuser');
        }

       // alert(userName);

        // On User Disconnected
        chatHub.client.onUserDisconnected = function (id, userName)
        {
            var ctrId = 'private_' + id;
            $('#' + ctrId).find('.msg_head .uname').text(userName + ": offline");
            console.log('onUserDisconnected');

         }
       
        chatHub.client.newMessageflash = function (userid) {
            var uid = $('#luserid').val();
            console.log('newMessageflash');
        }
          
        chatHub.client.getNewMessages = function (userName, message) {
            var pid = '';
            if (document.getElementById("toid") != null && document.getElementById("toid")!=undefined)
            {
                pid = document.getElementById("toid").value;
            }
            $.ajax({
                url: './Chat/MessageDisplay',
                type: 'POST',
                data: { id: pid },
                success: function (data) {                   
                    $('#MessageDisplay').html(data);
                    if ($('#PanelBodydiv')[0] != null && $('#PanelBodydiv')[0]!=undefined) {
                
                        var sheight = $('#PanelBodydiv')[0].scrollHeight;
                        if (sheight != null) {
                            $('#PanelBodydiv').scrollTop(sheight);
                        }
                    }        
                },
                error: function (req, status, error) {         
                },
            });
        }
        chatHub.client.online = function (allUsers) {
            $("#divusers").html('');
            for (i = 0; i < allUsers.length; i++)
            {
                AddUser(chatHub, allUsers[i].ConnectionId, allUsers[i].name, allUsers[i].userid, allUsers[i].avator,"Y");
            }
            console.log('online');
         //   alert(allUsers);
        };
        chatHub.client.isTyping = function (id, msg) {
            var ctrId = 'private_' + id;
            $('#' + ctrId).find('.msg_head .utyping').show();
            $('#' + ctrId).find('.msg_head .utyping').text('Typing...');
            setTimeout(function () {
                $('#' + ctrId).find('.msg_head .utyping').html('&nbsp;');
            }, 3000);
          
        }
        chatHub.client.removeMsg = function (msgid) {
            $('#MSG' + msgid).remove();

        }
        chatHub.client.sendPrivateMessage = function (msgid,windowId, fromUserName, message, msgdate, rs)
        {         
            var ctrId = 'private_' + windowId;
            if ($('#' + ctrId).length == 0)
            {                
              createPrivateChatWindow(chatHub, windowId, ctrId, fromUserName);
            }
            else
            {
                $('#' + ctrId).find('.msg_head .utyping').html('&nbsp;');
                $('#' + ctrId).parent('.msg_box').children('.msg_wrap').show();

                if (rs == 'sender')
                {
                    $('<div id="MSG'+msgid+'" class="msg_row_b msg_row"><div class="msg_b"><i data-item="' + msgid + '" class="fa fa-trash-o removemessage"></i>&nbsp;' + message + '<p class="chat-p"><i class="fa fa-calendar"></i>&nbsp;' + msgdate + '</p></div><div/>').insertBefore($('#' + ctrId).find('.msg_wrap .msg_push'));
                    $('#' + ctrId).find('.msg_body').scrollTop($('#' + ctrId).find('.msg_body')[0].scrollHeight);
                }
                else if (rs == 'receiver')
                {
                    $('<div id="MSG' + msgid + '" class="msg_row_a msg_row"><div class="msg_a"><i data-item="' + msgid + '" class="fa fa-trash-o removemessage"></i>&nbsp;' + message + '<p class="chat-p"><i class="fa fa-calendar"></i>&nbsp;' + msgdate + '</p></div></div>').insertBefore($('#' + ctrId).find('.msg_wrap .msg_push'));
                    $('#' + ctrId).find('.msg_body').scrollTop($('#' + ctrId).find('.msg_body')[0].scrollHeight);
                }
            }
        }

  


    }

    function AddUser(chatHub, id, name, uid,dp,isonline) {
 
        var userconId = $('#luserconid').val();
        var userId = $('#luserid').val();
        var code = "";
        //alert(userId);
        //alert(uid);
        if (userId == uid)
        {
        }
        else
        {
            var ctrId = 'private_' + uid;
            if ($('#' + ctrId).length > 0)
            {
              $('#' + ctrId).find('.msg_head .uname').text(name);
            }
            var flag = false;

            $('.user').each(function () {
                var tmpuid = $(this).attr('data-userid');

                if (tmpuid == uid)
                {
                    flag = true;
                    if (isonline == 'Y')
                    {
                        $(this).removeClass('offlineuser');
                        $(this).addClass('onlineuser');
                    }
                    else if (isonline == 'N')
                    {
                        $(this).removeClass('onlineuser');
                        $(this).addClass('offlineuser');
                    }
                }
            });
          
            if (flag == false)
            {
                if (isonline == 'Y') {
                    code = $('<div class="user onlineuser" data-userid="' + uid + '"> <img style="max-height: 28px;margin-right:5px;" src="' + dp + '" />' + name + '</div>');
                }
                else {
                    code = $('<div class="user offlineuser" data-userid="' + uid + '"> <img style="max-height: 28px;margin-right:5px;" src="' + dp + '" />' + name + '</div>');
                }
            }
            $(code).dblclick(function ()
            {
                var id = $(this).attr('data-userid');              
                if (userId != uid)
                OpenPrivateChatWindow(chatHub, id, name);
            });
        }
        $("#divusers").append(code);
    }
    function makeUseroffon(chatHub, id, name, uid, dp, isonline) {

        var userconId = $('#luserconid').val();
        var userId = $('#luserid').val();
        var code = "";
        if (userId == uid) {
        }
        else
        {
            var ctrId = 'private_' + uid;
            if ($('#' + ctrId).length > 0) {
                $('#' + ctrId).find('.msg_head .uname').text(name);
            }
            var flag = false;

            $('.user').each(function ()
            {
                var tmpuid = $(this).attr('data-userid');
                if (tmpuid == uid) {
                    flag = true;
                    if (isonline == 'Y') {
                        $(this).removeClass('offlineuser');
                        $(this).addClass('onlineuser');
                    }
                    else if (isonline == 'N') {
                        $(this).removeClass('onlineuser');
                        $(this).addClass('offlineuser');
                    }
                }
            });

        
        }
    }

    function AddMessage(userName, message)
    {
        
        message = message.replace(":)", "<img src=\"/demo/images/smile.gif\" class=\"smileys\" />");
        message = message.replace("lol", "<img src=\"/demo/images/laugh.gif\" class=\"smileys\" />");
        message = message.replace(":o", "<img src=\"/demo/images/cool.gif\" class=\"smileys\" />");        //message = message.replace("ass", "<img src=\"/demo/images/ass.gif\" class=\"smileys\" />");
       
        message = message.replace("love", "<img src=\"/demo/images/love.jpg\" class=\"smileys\" />");
        $('#divChatWindow').append('<div class="message"><span class="userName">' + userName + '</span>: ' + message + '</div>');
        var height = $('#divChatWindow')[0].scrollHeight;
        $('#divChatWindow').scrollTop(height);
    }

    function OpenPrivateChatWindow(chatHub, id, userName)
    {

        var ctrId = 'private_' + id;
        if ($('#' + ctrId).length > 0) {           
            $('#' + ctrId).find('.msg_head .uname').text(userName);
            return;
        }
        //$(".msg_box .emojionearea").remove();
        createPrivateChatWindow(chatHub, id, ctrId, userName);
    }
   
   
         function checkFileExtension(elem) {
             var filePath = elem.value;
             if (filePath.indexOf('.') == -1)
                 return false;
             var validExtensions = new Array();
             var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();

             validExtensions[0] = 'jpg';
             validExtensions[1] = 'jpeg';
             validExtensions[2] = 'bmp';
             validExtensions[3] = 'png';
             validExtensions[4] = 'gif';
             validExtensions[5] = 'docx';

             for (var i = 0; i < validExtensions.length; i++) {
                 if (ext == validExtensions[i])
                     return true;
             }
             alert('The file extension ' + ext.toUpperCase() + ' is not allowed!');
             elem.value = null;
             return false;
         }


    function showmyimage(fileInput, id) {
        if (checkFileExtension(fileInput) == true) {
            var files = fileInput.files;
            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                var imageType = /image.*/;
                if (!file.type.match(imageType)) {
                    continue;
                }
                var img = document.getElementById("thumbnil_" + id);
                
                //alert(img);

                img.file = file;
                var reader = new FileReader();
                reader.onload = (function (aImg) {
                    return function (e) {
                        aImg.src = e.target.result;
                    };
                })(img);

                $("#thumbnil_" + id).css("display", "block");

                $("#input_" + id).val(file);

                reader.readAsDataURL(file);
            }
        }
    }

   
        function createPrivateChatWindow(chatHub, userId, ctrId, userName) {
            for (var iii = 0; iii < popups.length; iii++) {
                //already registered. Bring it to front.
                if (ctrId == popups[iii]) {
                    Array.remove(popups, iii);
                    popups.unshift(ctrId);
                    calculate_popups();
                    return;
                }
            }

            $(document).ready(function () {
                $("#uploadimg").change(function () {
                    showmyimage(this, ctrId);                    
                });
            });
       
        var element = '<div class="msg_box" data-userid="' + userId + '" id="' + ctrId + '" >' +
                      '<div class="msg_head minimizeable"><span class="uname">' + userName +
                      //'</span>&nbsp;&nbsp;<a href="/Chat/ChatBox?userid=' + userId + '&chatid=' + ctrId + '&name=' + userName + '" target="_blank"><i class="fa fa-expand expandchat" aria-hidden="true"></i></a>&nbsp;&nbsp;<div  class="close">x</div><span style="display:block;" class="utyping"></span>' +
                      '</span>&nbsp;&nbsp;<a class="expandchat" href="javascript:void(0);"><i class="fa fa-expand" aria-hidden="true"></i></a>&nbsp;&nbsp;<div  class="close">x</div><span style="display:block;" class="utyping"></span>' +
                      '</div>' +
                      '<div class="msg_wrap">' +
                      '<div class="msg_body">' +
                  
                     '<div class="msg_push"></div>' +
                     '</div>' +
                     //'<div class="msg_footer"><textarea id="input_' + ctrId + '" class="msg_input" rows="4"></textarea></div>' +
                     '<div class="msg_footer"> <label class="attachmentimage" for="uploadimg"><span class="fa fa-paperclip" aria-hidden="true"></span>' +
                     '<input type="file" id="uploadimg" style="display:none"></label><textarea id="input_' + ctrId + '" class="msg_input" rows="4"></textarea>' +
                     '<img id="thumbnil_' + ctrId + '" src="/images/no-image.jpg" class="msg_input" style="display:none"/></div>' +
                     '</div>' +
                     '</div>';
     
                    document.getElementById('messageBoxCntainer').innerHTML += element
                    popups.unshift(ctrId);
                    calculate_popups();
                    var $div = $(element);
                    $(element).draggable();
                    // DELETE BUTTON IMAGE
                    if (userId!='')
                    {
                        getlist(userId);
                    }            
                    InitiateEmoji();
        }

    //this is used to close a popup
    function close_popup(id) {
        for (var iii = 0; iii < popups.length; iii++) {
            if (id == popups[iii]) {
                Array.remove(popups, iii);
               // document.getElementById(id).style.display = "none";
                calculate_popups();
                return;
            }
        }
    }

    //displays the popups. Displays based on the maximum number of popups that can be displayed on the current viewport width
    function display_popups() {
        var right = 290;
        var iii = 0;
        for (iii; iii < total_popups; iii++) {
            if (popups[iii] != undefined) {
                var element = document.getElementById(popups[iii]);
                element.style.right = right + "px";
                right = right + 290;
                element.style.display = "block";
            }
        }

        for (var jjj = iii; jjj < popups.length; jjj++) {
            var element = document.getElementById(popups[jjj]);
            element.style.display = "none";
        }
    }




    //calculate the total number of popups suitable and then populate the toatal_popups variable.
function calculate_popups() {
        var width = window.innerWidth;
        if (width < 540) {
            total_popups = 0;
        }
        else {
            width = width - 200;
            //320 is width of a single popup box
            total_popups = parseInt(width / 310);
        }
        display_popups();
    }
    //recalculate when window is loaded and also when window is resized.
window.addEventListener("resize", calculate_popups);
window.addEventListener("load", calculate_popups);
$(document).on('click', '.chat_head', function () {   
    $(this).parent('.chat_box').children('.chat_body').slideToggle();
    $(this).hide();
    $('.chatsticky').show();
});
$(document).on('click', '.chatsticky', function () {
    $(this).hide();
    $('.chat_head').parent('.chat_box').children('.chat_body').slideToggle();
    $('.chat_head').show();
});

$(document).on("click", '.msg_head', (function () {

    if ($(this).hasClass('minimizeable')) {
        $(this).parent('.msg_box').children('.msg_wrap').slideToggle('slow');
    }
}));


$(document).on("click", '#messageBoxCntainer .close', function () {  
    var ctrId = $(this).parents('#messageBoxCntainer .msg_box').attr("id");
    $(this).parents('#messageBoxCntainer .msg_box').remove();
    close_popup(ctrId);

});

$(document).on("click", '#messageBoxCntainer .expandchat', function () {
    var ctrId = $(this).parents('#messageBoxCntainer .msg_box').attr("id");
    var userid = $(this).parents('#messageBoxCntainer .msg_box').attr("data-userid");
    var uname = $(this).parents('#messageBoxCntainer .msg_box').find('.msg_head .uname').text();

    //window.location = '/Chat/ChatBox?userid=' + userid + '&chatid=' + ctrId + '&name=' + uname;
    //var win = window.open('/Chat/ChatBox?userid=' + userid + '&chatid=' + ctrId + '&name=' + uname, '_blank');
    //if (win) {
    //    //Browser has allowed it to be opened
    //    win.focus();
    //}

    javascript: survey_window = window.open('/Chat/ChatBox?userid=' + userid + '&chatid=' + ctrId + '&name=' + uname, 'Popup', 'width=450,height=400'); survey_window.focus();
   
    $(this).parents('#messageBoxCntainer .msg_box').remove();
    close_popup(ctrId);
});

$(document).on("click", '#messageBoxCntainer .removemessage', function () {
    var ctrId = $(this).parents('#messageBoxCntainer .msg_box').attr("id");
    var userid = $(this).parents('#messageBoxCntainer .msg_box').attr("data-userid");
    var uname = $(this).parents('#messageBoxCntainer .msg_box').find('.msg_head .uname').text();
    var msid = $(this).attr("data-item");
    chatHub.server.deleteMsg(msid);
    //$(this).parents('.msg_row').remove();
});





//$(document).on("keypress", '#messageBoxCntainer textarea',
//function (e) {
//    if (e.keyCode == 13) {
//        e.preventDefault();
//        var msg = $(this).val();
//        $(this).val('');
//        if (msg != null && msg.trim().length > 0) {
//            var userId = $(this).parents('#messageBoxCntainer .msg_box').attr("data-userid");
//            chatHub.state.userName = "John Doe1";
//            chatHub.server.sendPrivateMessage(userId, msg);
//        }
//    }
//    else
//    {
//        var userId = $(this).parents('#messageBoxCntainer .msg_box').attr("data-userid");
//        chatHub.server.userTyping(userId, msg);
//    }
    //});



//$(document).on("keypress", '#MessageBoxtxt',
//function (e) {
//    if (e.keyCode == 13) {
//        e.preventDefault();
//        var msg = $(this).val();
//        $(this).val('');
//        if (msg != null && msg.trim().length > 0) {
//            var pid = document.getElementById("toid").value;
//            var from = document.getElementById("fromid").value;
//            //var msg = document.getElementById("msg").value;
//            $.ajax({
//                url: './Chat/sendMessage',
//                type: 'POST',
//                data: { fromid: from, toid: pid, message: msg },
//                success: function (data) {
//                    chatHub.server.sendMsgToAll();
//                },
//                error: function (req, status, error) {
                    
//                },
//            });
            
//        }
//    }
//    else
//    {
//        //var userId = $(this).parents('#messageBoxCntainer .msg_box').attr("data-userid");
//        //chatHub.server.userTyping(userId, msg);
//    }
//});

//$(document).on("click", '#SendBtn',
//function (e) {
//    if (e.keyCode == 13)
//    {
//        e.preventDefault();
//        var msg = $(this).val();
//        $(this).val('');
//        if (msg != null && msg.trim().length > 0) {
//            var pid = document.getElementById("toid").value;
//            var from = document.getElementById("fromid").value;
//            //var msg = document.getElementById("msg").value;
//            $.ajax({
//                url: './Chat/sendMessage',
//                type: 'POST',
//                data: { fromid: from, toid: pid, message: msg },
//                success: function (data) {
//                    chatHub.server.sendMsgToAll();
//                },
//                error: function (req, status, error) {

//                },
//            });

//        }
//    }
//    else
//    {
//        //var userId = $(this).parents('#messageBoxCntainer .msg_box').attr("data-userid");
//        //chatHub.server.userTyping(userId, msg);
//    }
//});


//$("#fileinput").fileinput({
//    allowedFileExtensions: ["jpg", "png", "gif", "jpeg"],
//    maxImageWidth: 700,
//    maxImageHeight: 700,
//    resizePreference: 'height',
//    maxFileCount: 1,
//    resizeImage: true
//});


//$("#fileinput").on('fileloaded', function (event, file, previewId, index, reader) {


//    var readers = new FileReader();
//    readers.onloadend = function () {
//        $(".file-preview-image").attr('src', readers.result);
//    }
//    readers.readAsDataURL(file);
//});




//$('#btnSave').click(function () {
//    var imagesJson = $('.file-preview-image').map(function () {
//        var $this = $(this);
//        return {
//            image: $this.attr('src'),
//            filename: $this.attr('data-filename')
//        };
//    }).toArray();

//    itemHub.server.getByteArray(imagesJson);
//});
})