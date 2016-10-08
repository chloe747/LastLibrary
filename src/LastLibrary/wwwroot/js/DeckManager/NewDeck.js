$(function () {
    //create the LastLibrary namespace to store deck state
    if (window.LASTLIBRARY === undefined || window.LASTLIBRARY === null) window.LASTLIBRARY = {};

    //create a variable to track deck building data
    window.LASTLIBRARY.deckBuilder = {
        cards: {}
    };

    //apply click handler to the 'search for cards' button
    $("#cardSearchButton")
        .click(function (e) {
            e.preventDefault();
            requestCards(e);
        });

    //apply the enter key handler on the card name fieldcustom
    $('#cardName')
        .on('keypress',
            function (e) {
                if (e.keyCode === 13) {
                    e.preventDefault();
                    requestCards(e);
                }
            });

    //apply click handler to the 'save deck' button
    $("#deckSaveButton")
        .click(function (e) {
            e.preventDefault();
            saveDeckToServer();
        });
});

/**
 * Function that initiated and handles a call to the back-end to save the current deck
 * stored in the LASTLIBRARY namespace to the database
 */
function saveDeckToServer() {
    //get the current name in the name input element
    var deckName = $("#deckName").val();
    //get the other values from the input
    var description = $("#Description").val();
    var isPublic = $("#isPublic").is(":checked");

    //make sure there is a deckname
    if (deckName === undefined || deckName === null || deckName.length === 0) return null;

    //make sure we have card data, otherwise return
    if (window.LASTLIBRARY.deckBuilder.cards === undefined ||
        window.LASTLIBRARY.deckBuilder.cards === null ||
        Object.keys(window.LASTLIBRARY.deckBuilder.cards).length === 0) return null;

    //create the object to send to the server
    var requestContent = {
        deckName,
        description,
        isPublic,
        cards: $.map(window.LASTLIBRARY.deckBuilder.cards, function (cardObject) { return cardObject })
    };

    //send the request to the server
    $.ajax({
        type: "POST",
        url: "/api/Deck",
        contentType: 'application/json',
        data: JSON.stringify(requestContent),
        success: function (response) {
            console.log("deck saved!", response);
        },
        error: function () {
            console.error("Unable to save deck");
        }
    });
    return false;
}

/**
 * Function that gathers the currently entered form data, stores the data in a JSON and sends a request to the
 * card API hosted on our back-end.
 */
function requestCards() {
    //get the card name
    var cardName = $("#cardName").val();

    //get the card search results element
    var cardSearchResultsElement = $("#cardSearchResults");

    //get the appropriate value for the rarity
    var rarity = $("#rarity").val();
    if (rarity.toUpperCase() === 'ANY') {
        rarity = null;
    }

    //create the request object
    var requestContent = {
        "isblue": $("#colourBlue").is(":checked"),
        "isRed": $("#colourRed").is(":checked"),
        "isGreen": $("#colourGreen").is(":checked"),
        "iswhite": $("#colourWhite").is(":checked"),
        "isblack": $("#colourBlack").is(":checked"),
        "power": $("#power").val(),
        "toughness": $("#toughness").val(),
        "cmc": $("#mana").val(),
        "rarity": rarity
    };

    //before we send the data to the server, set a "loading" element in the search results
    var loadingElement = createLoadingElement();
    loadingElement.appendTo(cardSearchResultsElement);

    //send the Ajax call to the server
    $.ajax({
        type: "POST",
        url: "/api/card/" + cardName,
        contentType: 'application/json',
        data: JSON.stringify(requestContent),
        success: function (response) {
            //remove the previous search results
            cardSearchResultsElement.empty();

            //create and append the response to the results area
            response.cards.forEach(function (result) {

                //create the card element using the resulting card data
                var cardElement = createCardElement(result);

                //if we didn't get an element back, continue on to the next element
                if (cardElement === null || cardElement === undefined) return;

                //otherwise, append the card element to the results section
                cardElement.appendTo(cardSearchResultsElement);
            });
        },
        error: function () {
            console.error("Unable to query card api");
        }
    });
    return false;

}

/**
 * Function that creates DOM elements to display a magic the gathering card using the data passed into the function
 *
 * @@param {object} cardData - The card data used to display a M:tG card
 *
 * @@returns {element} - A DOM element that will contain the overall card element
 */
function createCardElement(cardData) {
    //check to see if we get an image URL, if we don't, then don't bother displaying the card
    if (cardData.imageUrl === undefined || cardData.imageUrl === null) return null;

    //otherwise, create the card elements
    var containingDiv = $("<div />",
    {
        "class": "cardResult noSelect"
    });
    var title = $("<p />",
    {
        "class": "cardTitle"
    });
    var image = $("<img />",
    {
        src: cardData.imageUrl,
        alt: cardData.name + " Image",
        "class": "card"
    });
    var plusIcon = $("<img />",
    {
        src: "/images/plusIcon.png",
        "class": "moveCard"
    });

    //append the elements together
    title.append(cardData.name);
    title.appendTo(containingDiv);
    plusIcon.appendTo(containingDiv);
    image.appendTo(containingDiv);


    //bind a Jquery .on click event to the plusIcon, and bind the card data to the icon
    plusIcon.on("click",
        cardData,
        function (e) {
            addCardToDeckBuilder(e.data);
            printCardsInDeckBuilder();
        });

    //return the final result
    return containingDiv;
}


/**
 * A function that adds a card to the deck builder's global state. if the card already exists, increment the card about by +1
 *
 * @@param {object} cardData - the data object for the card to be stored
 */
function addCardToDeckBuilder(cardData) {
    var cardId = cardData.multiverseId.toString();

    //when we get the card, check to see if the card already exists in the deck builder by checking the card's unique multiverse ID
    if (window.LASTLIBRARY.deckBuilder.cards[cardId] === undefined ||
        window.LASTLIBRARY.deckBuilder.cards[cardId] === null) {
        //create the object for the new multiverse ID
        window.LASTLIBRARY.deckBuilder.cards[cardId] = {};

        //add the new card to the deck builder and set the amount to 1
        window.LASTLIBRARY.deckBuilder.cards[cardId].card = cardData;

        //set the amount of cards that exist to 1
        window.LASTLIBRARY.deckBuilder.cards[cardId].amount = 1;
    } else {
        //otherwise, just increment the amount of cards by 1
        window.LASTLIBRARY.deckBuilder.cards[cardId].amount++;
    }
}

/**
 * A function that will decrement the amount of a card from the deck builder's global state.
 * if the card only has one copy, remove the entire card from the state
 *
 * @@param {object} cardData - the data object for the card to be removed
 */
function removeCardFromDeckBuilder(cardData) {
    var cardId = cardData.multiverseId.toString();

    //check to see if the card even exists in the namespace state
    if (window.LASTLIBRARY.deckBuilder.cards[cardId] === undefined ||
        window.LASTLIBRARY.deckBuilder.cards[cardId] === null) return null;

    //check to see if the card only has a single copy in the state, if it does, remove the entire entry
    if (window.LASTLIBRARY.deckBuilder.cards[cardId].amount === 1) {
        delete window.LASTLIBRARY.deckBuilder.cards[cardId];
    } else {
        //otherwise, just decrement the amount of cards by 1
        window.LASTLIBRARY.deckBuilder.cards[cardId].amount--;
    }
    return true;
}

/**
 * A function that will print out all the cards in the deckbuilder variable in the LASTLIBRARY namespace
 */
function printCardsInDeckBuilder() {
    //delete the current data in the 'current deck' section
    $("#currentDeck").empty();

    //make sure we have card data, otherwise return
    if (window.LASTLIBRARY.deckBuilder.cards === undefined ||
        window.LASTLIBRARY.deckBuilder.cards === null ||
        Object.keys(window.LASTLIBRARY.deckBuilder.cards).length === 0) return null;


    //iterate over all the cards in the current deckbuilder object and append them to the cards in deck section in the page
    $.each(window.LASTLIBRARY.deckBuilder.cards,
        function (cardName, cardObject) {
            createCardInDeck(cardObject)
                .appendTo($("#currentDeck"));
        });
    return false;
}

/**
 * Function that creates a set of elements that represent a card that
 * the user has chosen to be in thier deck
 *
 * @@ param {object} cardData -
 */
function createCardInDeck(cardData) {

    //create the containing div that will house the card data
    var container = $('<div />',
    {
        "class": "cardListItem lastLibrary-flex lastLibrary-flex--row lastLibrary-flex--justify-space-between"
    });
    //create the label for the cardname
    var cardLabel = $("<p />",
    {
        text: cardData.card.name + " x" + cardData.amount
    });
    //create the "minus" button
    var minusButton = $("<img />",
    {
        src: "/images/minusIcon.png",
        "class": "moveCard"
    });

    //attach the elements together
    cardLabel.appendTo(container);
    minusButton.appendTo(container);

    //bind a Jquery .on click event to the plusIcon, and bind the card data to the icon
    minusButton.on("click",
        cardData.card,
        function (e) {
            removeCardFromDeckBuilder(e.data);
            printCardsInDeckBuilder();
        });

    //return the containing element
    return container;
}

/**
 * Function that creates a loading element
 *
 * @@returns {element} - The loading element
 */
function createLoadingElement() {
    //empty the search results
    $("#cardSearchResults").empty();

    //TODO - Implement proper loading element, not just a text based element
    var loadingElement = $('<p />',
    {
        text: "Loading..."
    });
    return loadingElement;
}