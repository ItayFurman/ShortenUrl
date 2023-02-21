document.querySelector('#btnSubmit')
    .addEventListener('click', async e => {
        const temp = document.getElementById("url").value
        console.info(temp)
        await fetch("api/shorten", {
            method: "POST",
            body: JSON.stringify(temp),
            headers: { "Content-Type": "application/json" }

        }).then(Response => Response.text())
            .then(data => document.getElementById("shortUrl").value = data)

    })