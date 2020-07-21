
class Translator {

    _translate_month(month) {

        var result = month;

        switch (month) {
            case 'Jan':
                result = 'Janv';
                break;
            case 'Feb':
                result = 'Fév';
                break;
            case 'Mar':
                result = 'Mars';
                break;
            case 'Apr':
                result = 'Avr';
                break;
            case 'May':
                result = 'Mai';
                break;
            case 'Jun':
                result = "Juin";
                break;
            case 'Jul':
                result = "Juil";
                break;
            case 'Aug':
                result = 'Août';
                break;
            case 'Sep':
                result = 'Sept';
                break;
            case 'Oct':
                result = 'Oct';
                break;
            case 'Nov':
                result = 'Nov';
                break;
            case 'Dec':
                result = 'Déc';
                break;
        }

        return result;
    }

    _translate_day(day) {

        var result = day;

        switch (day) {
            case 'Monday':
                result = 'Lun';
                break;
            case 'Tuesday':
                result = 'Mar';
                break;
            case 'Wednesday':
                result = 'Mer';
                break;
            case 'Thursday':
                result = 'Jeu';
                break;
            case 'Friday':
                result = 'Ven';
                break;
            case 'Saturday':
                result = "Sam";
                break;
            case 'Sunday':
                result = "Dim";
                break;
        }

        return result;
    }

    translate_date_label(label) {

        if (!this.isFrenchLanguage()) {
            return label;
        }

        let month = label.match(/Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec/g);

        let day = label.match(/Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday/g);

        let translatedDate = label;

        if (month) {
            let translation = this._translate_month(month[0]);
            translatedDate = translatedDate.replace(month, translation);
        }

        if (day) {
            let translation = this._translate_day(day[0]);
            translatedDate = translatedDate.replace(day, translation);
        }

        return translatedDate;
    }

    isFrenchLanguage() {
        return navigator.language == "fr" || navigator.language == 'fr-FR';
    }

}
