window.addEventListener("DOMContentLoaded", function () {

    console.log("loaded");
    getAllProjects();

}, false);

function test() {
    let h = document.querySelector("h2");
    h.innerHTML = "test";
}

async function getLastTime() {
    const response = await fetch("/getLastDatetime", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        const v = (await response.json());

        document.querySelector("h2").innerHTML += String.prototype.concat(" | last time: ", v);
    }
}

async function getAllProjects() {
    // отправляет запрос и получаем ответ
    const response = await fetch("/getallprojects", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    // если запрос прошел нормально
    if (response.ok === true) {
        // получаем данные
        const projects = (await response.json()).$values;
        if (Array.isArray(projects) && projects.length == 0) {
            document.querySelector("h2").innerHTML += "  --> projects is empty";
        }
        else {
            const count = projects.filter(x => x.isNullAnySubFolderOrThisFolder).length;
            document.querySelector("h2").innerHTML += String.prototype.concat(" (projects with errors: ", count, ")");

                    }
        let rows = document.querySelector("tbody");
        projects.forEach(proj => {
            // добавляем полученные элементы в таблицу
            rows.append(row(proj));
        });
    }
    await getLastTime();
}

function row(p) {

    const tr = document.createElement("tr");

    const name = document.createElement("td");
    name.append(p.name);
    tr.append(name);

    const host = document.createElement("td");
    host.append(p.host);
    tr.append(host);

    const year = document.createElement("td");
    year.append(p.year);
    tr.append(year);

    const isnull = document.createElement("td");
    if (p.isNullAnySubFolderOrThisFolder) {
        isnull.setAttribute("class", "bg-danger")
    }
    else {
        isnull.setAttribute("class", "bg-success")

    }
    isnull.append(p.isNullAnySubFolderOrThisFolder);
    tr.append(isnull)

    return tr;
}