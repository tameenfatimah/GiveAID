document.getElementById('cardNumberInput').addEventListener('input', function () {
    var val = this.value.replace(/\D/g, '').substring(0, 16);
    var formatted = val.match(/.{1,4}/g);
    this.value = formatted ? formatted.join(' ') : val;
});

document.getElementById('cardExpiryInput').addEventListener('input', function () {
    var val = this.value.replace(/\D/g, '').substring(0, 4);
    if (val.length >= 2) {
        this.value = val.substring(0, 2) + '/' + val.substring(2);
    } else {
        this.value = val;
    }
});

document.querySelectorAll('.quick-amount').forEach(function (btn) {
    btn.addEventListener('click', function () {
        document.querySelector('input[name="amount"]').value = this.dataset.amount;
        document.querySelectorAll('.quick-amount').forEach(function (b) {
            b.classList.remove('btn-primary');
            b.classList.add('btn-outline-primary');
        });
        this.classList.remove('btn-outline-primary');
        this.classList.add('btn-primary');
    });
});