const form = document.getElementById('contactForm');
const password = document.getElementById('password');
const password2 = document.getElementById('password2');

form.addEventListener('submit', e => {
    validateInputs();

    if (isFormForPrevent()) {
        e.preventDefault();
    }
});

function isFormForPrevent() {

    if (document.getElementById('isPasswordValid').value == "error"
        || document.getElementById('isPassword2Valid').value == "error") {
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
    const passwordValue = password.value;
    const password2Value = password2.value;

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

    if (password2Value === '') {
        setError(password2, 'Password re-type is required');
        document.getElementById('isPassword2Valid').value = "error";
    } else if (passwordValue != password2Value) {
        setError(password2, 'The re-typed password must be same as the passwor')
        document.getElementById('isPassword2Valid').value = "error";
    } else {
        setSuccess(password2);
        document.getElementById('isPassword2Valid').value = "successfull";
    }
};