let stripe, customer, price, card, elements;
window.addEventListener('load', function () {
    stripe = window.Stripe('pk_test_51Q2DNf2L4K66u9tvGcuQV9KPwvSuJcwF3EVTNtBIQnHoX3VrX21cXVodQsBGiwFSvhVfZ2arUohyahSMBiafS4FU00YdP854MT');
    elements = stripe.elements();
});

let style = {
    base: {
        color: '#32325d',
        fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
        fontSmoothing: 'antialiased',
        fontSize: '16px',
        '::placeholder': {
            color: '#aab7c4'
        }
    },
    invalid: {
        color: '#fa755a',
        iconColor: '#fa755a'
    }
};
let startcard = true;
function Initiate() {
    if (startcard) {
        card = elements.create('card', { style: style });
        startcard = false;
    }
    card.mount('#card-element');
    card.on('change', function (event) {
        displayError(event);
    });
}

redirectToCheckout = function (sessionId) {
    var stripe = Stripe('pk_test_51Q2DNf2L4K66u9tvGcuQV9KPwvSuJcwF3EVTNtBIQnHoX3VrX21cXVodQsBGiwFSvhVfZ2arUohyahSMBiafS4FU00YdP854MT');
    stripe.redirectToCheckout({
        sessionId: sessionId
    }).then(function (result) {
        if (result.error) {
            var displayError = document.getElementById("stripe-error");
            displayError.textContent = result.error.message;
        }
    });
};

function displayError(event) {

    let displayError = document.getElementById('card-element-errors');

    if (event.error) {
        displayError.textContent = event.error.message;
    } else {
        displayError.textContent = '';
    }
}

function createPaymentMethod(dotnetHelper, cardElement, billingemail, billingName) {
    let res = stripe
        .createPaymentMethod({
            type: 'card',
            card: cardElement,
            billing_details: {
                name: billingName,
                email: billingemail,
            },
        })
        .then((result) => {
            if (result.error) {
                displayError(result);
            }
            else {
                createSubscription(dotnetHelper, result.paymentMethod.id);
            }
        });
    return;
}

function createPaymentMethodServer(dotnetHelper, billingemail, billingName) {
    createPaymentMethod(dotnetHelper, card, billingemail, billingName);
}

function createSubscription(dotnetHelper, paymentMethodId) {
    dotnetHelper.invokeMethodAsync('Subscribe', paymentMethodId);
    dotnetHelper.dispose();
}