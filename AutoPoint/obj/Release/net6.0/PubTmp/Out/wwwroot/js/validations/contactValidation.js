const form = document.getElementById('contactForm');
const name = document.getElementById('name');
const email = document.getElementById('email');
const subject = document.getElementById('subject');
const message = document.getElementById('message');

form.addEventListener('submit', e => { 
    validateInputs();

    if (isFormForPrevent()) {
       e.preventDefault();
    }
});

function isFormForPrevent() {

    if (document.getElementById('isNameValid').value == "error"
        || document.getElementById('isEmailValid').value == "error"
        || document.getElementById('isSubjectValid').value == "error"
        || document.getElementById('isMessageValid').value == "error") {
        return true;
    } else {
        return false;
    }
}

const isValidEmail = email => {
    const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
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
    const emailValue = email.value;
    const subjectValue = subject.value;
    const messageValue = message.value;

    if (nameValue === '') {
        setError(name, 'Name is required');
        document.getElementById('isNameValid').value = "error";
    } else if (nameValue.length <= 8) {
        setError(name, 'Full name must be at least 8 character.');
        document.getElementById('isNameValid').value = "error";
    } else {
        setSuccess(name);
        document.getElementById('isNameValid').value = "successfull";
    }

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

    if (subjectValue === '') {
        setError(subject, 'Subject is required');
        document.getElementById('isSubjectValid').value = "error";
    } else if (subjectValue.length <= 8) {
        setError(subject, 'Subject must be at least 8 character.');
        document.getElementById('isSubjectValid').value = "error";
    } else {
        setSuccess(subject);
        document.getElementById('isSubjectValid').value = "successfull";
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