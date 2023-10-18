const form = document.getElementById('contactForm');
const fullName = document.getElementById('fullName');
const message = document.getElementById('message');

form.addEventListener('submit', e => {
    validateInputs();

    if (isFormForPrevent()) {
        e.preventDefault();
    }
});

function isFormForPrevent() {

    if (document.getElementById('isFullNameValid').value == "error"
        || document.getElementById('isMessageValid').value == "error") {
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
    const fullNameValue = fullName.value;
    const messageValue = message.value;

    if (fullNameValue === '') {
        setError(fullName, 'Name is required');
        document.getElementById('isFullNameValid').value = "error";
    } else if (fullNameValue.length <= 8) {
        setError(fullName, 'Full name must be at least 8 character.');
        document.getElementById('isFullNameValid').value = "error";
    } else {
        setSuccess(fullName);
        document.getElementById('isFullNameValid').value = "successfull";
    }

    if (messageValue === '') {
        setError(message, 'Message is required');
        document.getElementById('isMessageValid').value = "error";
    } else if (messageValue.length < 10) {
        setError(message, 'Message must be at least 10 character.')
        document.getElementById('isMessageValid').value = "error";
    } else {
        setSuccess(message);
        document.getElementById('isMessageValid').value = "successfull";
    }
};