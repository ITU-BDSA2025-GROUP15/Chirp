/**
 * 
 * @param {Event} event 
 */
const onLikeSubmit = function(event) {
    event.preventDefault()
    let formEl = event.target;
    let textEl = formEl.querySelector(".like-btn-text");
    let counterEl = formEl.querySelector(".like-btn-counter");

    let token = formEl.querySelector('input[name="__RequestVerificationToken"]').value;

    fetch(formEl.action + "&json=true", {
        method: "POST",
        headers: {
            "RequestVerificationToken": token
        },
    })
    .then(response => response.json())
    .then(data => {
        console.log(data);
        counterEl.textContent = data.likeCount;
        textEl.textContent = data.hasLiked ? "liked!" : "like";
    });
}

document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".like-btn-form").forEach(btnForm => {
        btnForm.addEventListener("submit", onLikeSubmit);
    });
});