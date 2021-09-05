﻿var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();
connection.start();

var pageData = {
	selectedUserId: null,
	currentUserId: $("#currentUserId").val()
};

$(".user-item").click(function (ev) {
	// Xóa thẻ đc chọn trước đó
	$(".user-item.selected").removeClass("selected");
	$(ev.currentTarget).addClass("selected");
	// Lưu lại id của user đc chọn
	pageData.selectedUserId = $(ev.currentTarget).attr("data-user-id");
});

$("#input-msg").keydown(function (ev) {
	if (ev.keyCode == 13 && ev.shiftKey == false) {
		// Lấy nội dung tin nhắn
		var mesg = $(ev.currentTarget).val();
		// Gửi tin nhắn
		connection.invoke("GuiTinNhan", pageData.selectedUserId, mesg)
			.then(function () {
				// Sau khi gửi tin nhắn thì xóa tin nhắn đó
				$(ev.currentTarget).val("");
			});
	}
});
// Sự kiện nhận tin nhắn
connection.on("PhanHoiTinNhan", function (response) {
	var template = `<div class="msg-box">
						<div class="msg-content">${response.mesg}</div>
						<div class="msg-time">${response.datetime}</div>
					</div>`;
	var element = $(template);

	// Nếu đây là tin nhắn của mình gửi
	if (pageData.currentUserId == response.sender) {
		element.addClass("me");
	}

	var container = $(".msg-box-container");
	container.append(element);

	// Lăn xuống cuối
	container.scrollTop(container[0].scrollHeight);
});

// Sự kiện khi có user online
connection.on("GetUsers", function (response) {
	for (var i = 0; i < response.onlineUsers.length; i++) {
		var id = response.onlineUsers[i];
		$(`.user-item[data-user-id=${id}] > .user-fullname`)
			.addClass("online");
	}

	// Nếu reponse có thuộc tính disconnectedId =>user offline
	if (response.disconnectedId) {
		$(`.user-item[data-user-id=${response.disconnectedId}] > .user-fullname`)
			.removeClass("online");
	}
});