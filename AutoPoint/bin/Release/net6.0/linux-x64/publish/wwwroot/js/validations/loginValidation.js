const form = document.getElementById('contactForm');
const email = document.getElementById('email');
const password = document.getElementById('password');

form.addEventListener('submit', e => {
    validateInputs();

    if (isFormForPrevent()) {
        e.preventDefault();
    }
});

function isFormForPrevent() {

    if (document.getElementById('isEmailValid').value == "error"
        || document.getElementById('isPasswordValid').value == "error") {
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

const isValidEmail = email => {
    const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}

const validateInputs = () => {
    const emailValue = email.value.trim();
    const passwordValue = password.value.trim();

    if (emailValue === '') {
        setError(email, 'Email is required');
        document.getElementById('isEmailValid').value = "error";
    } else if (!isValidEmail(emailValue)) {
        setError(email, 'Provide a valid email address');
        document.getElementById('isEmailValid').value = "error";
    } else {
        setSuccess(email);
        document.getElementById('isEmailValid').value = "successfull";
    }

    if (passwordValue === '') {
        setError(password, 'Password is required');
        document.getElementById('isPasswordValid').value = "error";
    } else if (passwordValue.length < 8) {
        setError(password, 'Password must be at least 8 character.')
        document.getElementById('isPasswordValid').value = "error";
    } else {
        setSuccess(password);
        document.getElementById('isPasswordValid').value = "successfull";
    }
};