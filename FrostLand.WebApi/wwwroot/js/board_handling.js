function insert_boards(id, parent) {

    if (parent == 'undefined')
        parent = -1;

    $.getJSON("/api/board/getboards" + '?parent=' + parent, function (data) {
        for (var index in data) {
            var name = document.createElement('div');
            name.className = "board-name";
            name.innerHTML = data[index].name;

            var desc = document.createElement('div');
            desc.className = "board-description";
            desc.innerHTML = data[index].description;

            var board = document.createElement('div');
            board.className = "board";
            board.appendChild(name);
            board.appendChild(desc);

            board.onclick = function () { get_board(data[index].id); };
            //board.addEventListener("click", get_board(data[index].id));

            $(id).append(board);
        }
    });
}

function get_board(boardId) {
    window.location.href = "/api/board/getboard" + '?id=' + boardId;
}