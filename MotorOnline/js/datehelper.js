function addDays(date, days) {
    var result = new Date(date);
    result.setDate(date.getDate() + days);
    return result;
}

function AddDate(oldDate, offset, offsetType) {
    oldDate = new Date(oldDate);
    var year = parseInt(oldDate.getFullYear());
    var month = parseInt(oldDate.getMonth());
    var date = parseInt(oldDate.getDate());
    var hour = parseInt(oldDate.getHours());
    var newDate;
    switch (offsetType) {
        case "Y":
        case "y":
            newDate = new Date(year + offset, month, date, hour);
            break;

        case "M":
        case "m":
            newDate = new Date(year, month + offset, date, hour);
            break;

        case "D":
        case "d":
            newDate = new Date(year, month, date + offset, hour);
            break;

        case "H":
        case "h":
            newDate = new Date(year, month, date, hour + offset);
            break;
    }

    return newDate;
}


function handlecalendarselect() {
    //CHANGE: 1/7/2014
    //Policy Period from: Can Back date up to 6 days only. Sample if today is Jan 7 2014
    //you can only pick Jan 2, 2014. forward date can accept. 

    var value = $('#PeriodFromTextbox').val();
    var addedDate = addDays(new Date(), -6);
    //var currentDate = new Date();
    //var exactCurrentDate = Date.parse(currentDate.getMonth() + 1 + '/' + formatdate(currentDate.getDate()) + '/' + currentDate.getFullYear());
    if (Date.parse(value) < Date.parse(addedDate)) {
        alert('The acceptable date is (6) six ago from the date today.');
        $('#PeriodToTextbox').val('');
        $('#PeriodFromTextbox').val('');
        return;
    }
    
//     else if (Date.parse(value) < new Date(exactCurrentDate)) {
//        alert('Cannot select past date.');
//        $('#PeriodToTextbox').val('');
//        $('#PeriodFromTextbox').val('');
//        return;
//    }
    else {
        var endDate = AddDate(value, 1, "Y");
        $('#PeriodToTextbox').val(endDate.getMonth() + 1 + '/' + formatdate(endDate.getDate()) + '/' + endDate.getFullYear());
    }
}

function formatdate(date) {
    if (date < 10) {
        date = '0' + date;
    }
    return date;
}