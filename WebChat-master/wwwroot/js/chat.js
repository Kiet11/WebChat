var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();
connection.start();

var pageData = {
	selectedUserId: null,
	currentUserId: $("#currentUserId").val(),
	conversation: []
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
	// Tạo phần tử html từ string ở trên
	var element = $(template);
	var container = $(".msg-box-container");

	var convs = pageData.conversation;
	var myId = pageData.currentUserId;
	var selectedId = pageData.selectedUserId;
	// Lưu lại tin nhắn cho cuộc trò chuyện hiện tại
	// Nếu mình là người gửi tin nhắn
	if (myId == response.sender) {
		// Nếu đây là tin nhắn của mình gửi thì thêm class "me"
		element.addClass("me");
		container.append(element);
		// Lăn xuống cuối
		container.scrollTop(container[0].scrollHeight);

		if (convs[selectedId] == null) {
			convs[selectedId] = [];
		}
		convs[selectedId].push(response);
	}
	else if (myId == response.reciver) {
		if (selectedId == response.sender) {
			// Tin nhắn từ người khác gửi tới
			container.append(element);
			// Lăn xuống cuối
			container.scrollTop(container[0].scrollHeight);
		}

		if (convs[response.sender] == null) {
			convs[response.sender] = [];
		}
		

		convs[response.sender].push(response);
	}
});
//Sự kiện khi có user-online
connection.on("GetUsers", function (response) {
	for (var i = 0; i < response.onlineUsers.length; i++) {
		var id = response.onlineUsers[i];
		$(`.user-item[data-user-id=${id}] > .user-fullname`)
			.addClass("online");

	}
	if (response.disconnectedId) {
		$(`.user-item[data-user-id=${response.disconnectedId}] > .user-fullname`)
			.removeClass("online");
	}
});