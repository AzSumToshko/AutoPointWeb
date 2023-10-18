const form = document.getElementById('checkout-form');
const firstName = document.getElementById('firstName');
const lastName = document.getElementById('lastName');
const address = document.getElementById('address');
const email = document.getElementById('email');
const phoneNumber = document.getElementById('phoneNumber');
const city = document.getElementById('city');
const zip = document.getElementById('zip');

form.addEventListener('submit', e => {
    validateInputs();

    if (isFormForPrevent()) {
        e.preventDefault();
    }
});

function isFormForPrevent() {

    if (document.getElementById('isFirstNameValid').value == "error"
        || document.getElementById('isLastNameValid').value == "error"
        || document.getElementById('isEmailValid').value == "error"
        || document.getElementById('isAddressValid').value == "error"
        || document.getElementById('isPhoneNumberValid').value == "error"
        || document.getElementById('isCityValid').value == "error"
        || document.getElementById('isZipValid').value == "error") {
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
    const firstNameValue = firstName.value;
    const lastNameValue = lastName.value;
    const addressValue = address.value;
    const emailValue = email.value;
    const phoneNumberValue = phoneNumber.value;
    const cityValue = city.value;
    const zipValue = zip.value;

    if (firstNameValue === '') {
        setError(firstName, 'First name is required');
        document.getElementById('isFirstNameValid').value = "error";
    } else if (firstNameValue.length < 4) {
        setError(firstName, 'First name must be at least 4 character.');
        document.getElementById('isFirstNameValid').value = "error";
    } else {
        setSuccess(firstName);
        document.getElementById('isFirstNameValid').value = "successfull";
    }

    if (lastNameValue === '') {
        setError(lastName, 'Last name is required');
        document.getElementById('isLastNameValid').value = "error";
    } else if (lastNameValue.length < 4) {
        setError(lastName, 'Last name must be at least 4 character.');
        document.getElementById('isLastNameValid').value = "error";
    } else {
        setSuccess(lastName);
        document.getElementById('isLastNameValid').value = "successfull";
    }

    if (addressValue === '') {
        setError(address, 'Address is required');
        document.getElementById('isAddressValid').value = "error";
    } else if (addressValue.length < 10) {
        setError(address, 'Address must be at least 10 character.');
        document.getElementById('isAddressValid').value = "error";
    } else {
        setSuccess(address);
        document.getElementById('isAddressValid').value = "successfull";
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

    if (phoneNumberValue === '') {
        setError(phoneNumber, 'Phone number is required');
        document.getElementById('isPhoneNumberValid').value = "error";
    } else if (phoneNumberValue.length < 10) {
        setError(phoneNumber, 'Phone number must be at least 10 character.');
        document.getElementById('isPhoneNumberValid').value = "error";
    } else {
        setSuccess(phoneNumber);
        document.getElementById('isPhoneNumberValid').value = "successfull";
    }

    if (cityValue === '') {
        setError(city, 'City is required');
        document.getElementById('isCityValid').value = "error";
    } else if (cityValue.length < 4) {
        setError(city, 'City must be at least 4 character.');
        document.getElementById('isCityValid').value = "error";
    } else {
        setSuccess(city);
        document.getElementById('isCityValid').value = "successfull";
    }

    if (zipValue === '') {
        setError(zip, 'Zip is required');
        document.getElementById('isZipValid').value = "error";
    } else if (zipValue.length < 4) {
        setError(zip, 'Zip must be at least 4 character.');
        document.getElementById('isZipValid').value = "error";
    } else {
        setSuccess(zip);
        document.getElementById('isZipValid').value = "successfull";
    }
};