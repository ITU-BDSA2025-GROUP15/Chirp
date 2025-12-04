document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".like-btn").forEach(btn => {
        btn.addEventListener("click", function () {

            let id = this.dataset.id;
            let likeUrl = this.dataset.url;   // comes from the view
            let token = document.querySelector('input[name="__RequestVerificationToken"]').value;

            let currentText = document.getElementById("likeBtn-" + id).textContent;

            fetch(`${likeUrl}&id=${id}`, {
                method: "POST",
                headers: {
                    "RequestVerificationToken": token
                }
            })
                .then(response => response.json())
                .then(data => {
                    console.log(data);
                    document.getElementById("likeCount-" + id).textContent = data.likeCount;
                    document.getElementById("likeBtn-" + id).textContent =
                        data.hasLiked ? "Unlike" : "Like";
                });
        });
    });
});