
// Get all the radio buttons and labels
const ratingRadios = $('.star-rating input[type="radio"]');
const ratingLabels = $('.star-rating label');
const ratingForm = $('#rating-form');
const submitButton = $('#submit-rating');
const itemId = $('#itemId').val();
// Add event listener to the radio buttons
ratingRadios.on('change', function () {
    submitButton.prop('disabled', false);
    // Remove the 'selected' class from all labels
    ratingLabels.removeClass('selected');
    // Add the 'selected' class to the current and all previous labels
    $(this).prevAll('label').add(this).addClass('selected');
});
// Add click event listener to the submit button
submitButton.on('click', function () {
    submitRatingViaAjax();
});
function submitRatingViaAjax() {
    const ratingValue = $('.star-rating input[name="rating"]:checked').val();
    $.ajax({
        url: `/Items/RateProduct?ratingValue=${ratingValue}&itemId=${itemId}`,
        method: 'POST',
        success: function (response) {
            // Handle the successful response
            console.log('Rating submitted successfully');
            console.log(ratingValue);
        },
        error: function (xhr, status, error) {
            // Handle any errors
            console.error('Error submitting rating:', error);
        }
    });
}
