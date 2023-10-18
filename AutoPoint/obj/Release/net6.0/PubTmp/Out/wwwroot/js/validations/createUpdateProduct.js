const form = document.getElementById('contactForm');
const name = document.getElementById('name');
const description = document.getElementById('description');
const price = document.getElementById('price');

form.addEventListener('submit', e => {
    validateInputs();

    if (isFormForPrevent()) {
        e.preventDefault();
    }
});

function isFormForPrevent() {

    if (document.getElementById('isNameValid').value == "error"
        || document.getElementById('isDescriptionValid').value == "error"
        || document.getElementById('isPriceValid').value == "error") {
        return true;
    } else {
        return false;
    }
}

const setError = (element, message) => {
    const inputControl = element.parentElement;
    const errorDisplay = inputControl.querySelector('.error');

    errorDisplay.innerText = message;
    inputControl.classList.add('error');
    inputControl.classList.remove('success')
}

const setSuccess = element => {
    const inputControl = element.parentElement;
    const errorDisplay = inputControl.querySelector('.error');

    errorDisplay.innerText = '';
    inputControl.classList.add('success');
    inputControl.classList.remove('error');
};

const validateInputs = () => {
    const nameValue = name.value;
    const descriptionValue = description.value;
    const priceValue = price.value;

    if (nameValue === '') {
        setError(name, 'Name is required');
        document.getElementById('isNameValid').value = "error";
    } else if (nameValue.length <= 4) {
        setError(name, 'Product name must be at least 4 character.');
        document.getElementById('isNameValid').value = "error";
    } else {
        setSuccess(name);
        document.getElementById('isNameValid').value = "successfull";
    }

    if (descriptionValue === '') {
        setError(description, 'Description is required');
        document.getElementById('isDescriptionValid').value = "error";
    } else if (descriptionValue.length < 10) {
        setError(description, 'Product description must be at least 10 character.');
        document.getElementById('isDescriptionValid').value = "error";
    } else {
        setSuccess(description);
        document.getElementById('isDescriptionValid').value = "successfull";
    }

    if (priceValue === '') {
        setError(price, 'Price is required');
        document.getElementById('isPriceValid').value = "error";
    } else if (priceValue < 1) {
        setError(price, 'Price must be positive number');
        document.getElementById('isPriceValid').value = "error";
    } else {
        setSuccess(price);
        document.getElementById('isPriceValid').value = "successfull";
    }
};