function allowDrop(ev) {
    ev.preventDefault();
}

function drag(ev) {
    ev.dataTransfer.setData("text", ev.target.id);
    ev.target.classList.add('dragging');
}

function drop(ev) {
    ev.preventDefault();
    var data = ev.dataTransfer.getData("text");
    var tile = document.getElementById(data);
    var column = ev.target.closest('.column');

    if (tile && column) {
        column.querySelector('.tiles').appendChild(tile);
        tile.classList.remove('dragging');

        fetch('/api/update-tile-position', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                id: parseInt(data.replace('tile-', '')),
                newStatus: column.dataset.status
            })
        })
            .then(response => {
                if (response.status === 204) {
                    return {};
                }
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.text();
            })
            .then(text => {
                if (text) {
                    try {
                        const jsonData = JSON.parse(text);
                        if (jsonData.success) {
                            console.log('Status updated successfully.');
                        } else {
                            console.error('Failed to update status:', jsonData.message);
                        }
                    } catch (e) {
                        console.error('Failed to parse JSON:', e);
                    }
                } else {
                    console.log('No response content.');
                }
            })
            .catch(error => {
                console.error('Error:', error);
            });
    }
}

function openModal(modalId) {
    var modalElement = document.getElementById(modalId);
    if (modalElement) {
        var modal = new bootstrap.Modal(modalElement);
        modal.show();
    } else {
        console.error("Modal element not found with ID:", modalId);
    }
}