
function phoneValidate(input) {
    input.value = input.value.replace(/[^0-9]/g, '').replace(/(\..*)\./g, '$1');
}

function justNumberValidate(input) {
    input.value = input.value.replace(/[^0-9]/g, '').replace(/(\..*)\./g, '$1');
}

function phoneValidatekeyup(input) {
    if (input.value === '0') {
        input.value = input.value.substr(1);
    }
}
function justTextValidatekeyup(input) {
    input.value = input.value.replace(/[^a-zA-ZŞşÇçİÜüĞğÖö\s]/g, '');
}

const ccInputElement = document.querySelector('#creditCardNumber');

ccInputElement.format = () => {
    let cursorPosition = ccInputElement.selectionStart;
    let partBeforeCursorPosition = ccInputElement.value.substring(0, cursorPosition);
    let partAfterCursorPosition = ccInputElement.value.substring(cursorPosition);

    const originalLength = partBeforeCursorPosition.length;
    if (originalLength > 16) {
        partBeforeCursorPosition = ccInputElement.value.replace(/\s+/g, '').substring(0, 16);
    }
    partBeforeCursorPosition = partBeforeCursorPosition.replace(/[^0-9]/g, '');
    partBeforeCursorPosition = partBeforeCursorPosition.replace(/\s/gi, '');
    cursorPosition -= originalLength - partBeforeCursorPosition.length;
    partAfterCursorPosition = partAfterCursorPosition.replace(/\s/gi, '');
    const ccNumber = partBeforeCursorPosition + partAfterCursorPosition;
    const parts = ccNumber.match(/.{1,4}/g);

    ccInputElement.value = parts?.join(' ') || '';
    cursorPosition += Math.floor(cursorPosition * 1 / 4);
    ccInputElement.setSelectionRange(cursorPosition, cursorPosition);
    document.getElementById("creditCardNumber").value = ccInputElement.value.replace(/\s+/g, '');
};

ccInputElement.addEventListener('input', ccInputElement.format);

ccInputElement.addEventListener('keydown', event => {
    const cursorPosition = ccInputElement.selectionStart;

    if (event.key == 'Backspace') {
        if (cursorPosition == ccInputElement.selectionEnd
            && ccInputElement.value[cursorPosition - 1] == ' ') {
            event.preventDefault();
            const newCursorPosition = cursorPosition - 2;
            ccInputElement.value = ccInputElement.value.substring(0, newCursorPosition) + ccInputElement.value.substring(cursorPosition);
            ccInputElement.setSelectionRange(newCursorPosition, newCursorPosition);
            ccInputElement.format();
        }
    }
    else if (event.key == 'ArrowRight') {
        if (ccInputElement.value[cursorPosition + 1] == ' ') {
            const newCursorPosition = cursorPosition + 1;
            ccInputElement.setSelectionRange(newCursorPosition, newCursorPosition);
        }
    }
    else if (event.key == 'ArrowLeft') {
        if (ccInputElement.value[cursorPosition - 1] == ' ') {
            const newCursorPosition = cursorPosition - 1;
            ccInputElement.setSelectionRange(newCursorPosition, newCursorPosition);
        }
    }
});

function onlyNumberKey(input) {
    var ASCIICode = (input.which) ? input.which : input.keyCode
    if (ASCIICode > 31 && (ASCIICode < 48 || ASCIICode > 57))
        return false;
    return true;
}

function limitMaxValue(element) {
    let max = parseInt(element.max);
    let min = parseInt(element.min);
    let value = parseInt(element.value);
    if (value > max) {
        element.value = max;
    }
    else if (value < min) {
        element.value = min;
    }
}
function handle_form_submission() {
    alert('Submit button pressed');
    return false; //do not submit the form
}

var $form = $("#myform");
var $submitbutton = $("#submitbutton");

$form.on("blur", "input", () => {
    if ($form.valid()) {
        $submitbutton.removeAttr("disabled");
    } else {
        $submitbutton.attr("disabled", "disabled");
    }
});
