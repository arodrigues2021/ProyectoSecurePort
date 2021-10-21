$(function () {
    $(".borrar").click(function () {
        return confirm(mensajeDelete);
    });

    $("#CheckAll").click(function () {
        $(".chequeable").attr("checked", true);
    });

    $("#CheckNothing").click(function () {
        $(".chequeable").attr("checked", false);
    });

    $('#wysiwyg').wysiwyg({
        controls: {
            strikeThrough: { visible: true },
            underline: { visible: true },

            separator00: { visible: true },

            justifyLeft: { visible: true },
            justifyCenter: { visible: true },
            justifyRight: { visible: true },
            justifyFull: { visible: true },

            separator01: { visible: true },

            indent: { visible: true },
            outdent: { visible: true },

            separator02: { visible: true },

            subscript: { visible: true },
            superscript: { visible: true },

            separator03: { visible: true },

            undo: { visible: true },
            redo: { visible: true },

            separator04: { visible: true },

            insertOrderedList: { visible: true },
            insertUnorderedList: { visible: true },
            insertHorizontalRule: { visible: true },

            h4mozilla: { visible: true && $.browser.mozilla, className: 'h4', command: 'heading', arguments: ['h4'], tags: ['h4'], tooltip: "Header 4" },
            h5mozilla: { visible: true && $.browser.mozilla, className: 'h5', command: 'heading', arguments: ['h5'], tags: ['h5'], tooltip: "Header 5" },
            h6mozilla: { visible: true && $.browser.mozilla, className: 'h6', command: 'heading', arguments: ['h6'], tags: ['h6'], tooltip: "Header 6" },

            h4: { visible: true && !($.browser.mozilla), className: 'h4', command: 'formatBlock', arguments: ['<H4>'], tags: ['h4'], tooltip: "Header 4" },
            h5: { visible: true && !($.browser.mozilla), className: 'h5', command: 'formatBlock', arguments: ['<H5>'], tags: ['h5'], tooltip: "Header 5" },
            h6: { visible: true && !($.browser.mozilla), className: 'h6', command: 'formatBlock', arguments: ['<H6>'], tags: ['h6'], tooltip: "Header 6" },

            separator07: { visible: true },

            cut: { visible: true },
            copy: { visible: true },
            paste: { visible: true }
        }
    });
})

Array.prototype.contains = function (element) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == element) {
            return true;
        }
    }
    return false;
}

String.prototype.splitToArray = function (c) {
    var arr = new Array();
    var ind = 0;
    var indArr = 0;
    var aux = '';

    while (ind < this.length) {
        if (this.charAt(ind) != c) {
            aux = aux.concat(this.charAt(ind));
        }
        else {
            arr[indArr] = aux;
            aux = '';
            indArr++;
        }
        ind++;
    }
    arr[indArr] = aux;
    return arr;
}

function esNumerico(input) {
    return (input - 0) == input && input.length > 0;
}

function fecha_correcta(fecha) {
    // fecha máxima: 31/12/2099
    // fecha mínima: 01/01/2001
    var af = fecha.splitToArray('/');
    if (af.length != 3) {
        return "Today";
    }
    else {
        var year = parseInt(af[2]);
        var month = parseInt(af[1]);
        var day = parseInt(af[0]);
        if (isNaN(year) || isNaN(month) || isNaN(day)) {
            return "Today";
        }
        else {
            if (year > 2099) { year = 2099; }
            else if (year < 2001) { year = 2001; }

            if (month > 12) { month = 12; }
            else if (month < 1) { month = 1; }

            if (day < 1) { day = 1; }
            else {
                if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) {
                    if (day > 31) { day = 31; }
                }
                else if (month == 4 || month == 6 || month == 9 || month == 11) {
                    if (day > 30) { day = 30; }
                }
                else {
                    if (year % 4 == 0) {
                        if (day > 29) { day = 29; }
                    }
                    else {
                        if (day > 28) { day = 28; }
                    }
                }
            }
            return day.toString() + '/' + month.toString() + '/' + year.toString();
        }
    }
}

function solo_numeros(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57)) return false;
    return true;
}

function solo_numeros_decimales(evt) {
    var resultado = true;
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57)) { resultado = false; }
    if (charCode == 44) { resultado = true; }
    return resultado;
}

function nombre_usuario(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode <= 31 || charCode == 95 || (charCode >= 48 && charCode <= 57) || (charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122)) return true;
    return false;
}


function bloquearBotones(cambio, maximo) {
    if (maximo == 0) {
        bloquearBotonAnterior(true);
        bloquearBotonSiguiente(true);
    }
    else if (cambio == 0) {
        bloquearBotonAnterior(true);
        bloquearBotonSiguiente(false);
    }
    else if (cambio > 0 && cambio < maximo) {
        bloquearBotonAnterior(false);
        bloquearBotonSiguiente(false);
    }
    else if (cambio == maximo) {
        bloquearBotonAnterior(false);
        bloquearBotonSiguiente(true);
    }
}

function sortNumber(a, b) {
    return a - b;
}

function limpiarYOrdenarArray(arr) {
    arr.sort(sortNumber);

    while (arr.slice(arr.length - 1, arr.length) == '') {
        arr.pop();
    }

    return arr;
}

function existe_en_array(str, val) {
    return contains(str, val);
}

function contains(str, obj) {
    var arrStr = (str.toString()).splitToArray(',');
    return arrStr.contains(obj);
}

function devolverPosicionEnArray(arr, obj) {
    var elementos = (arr.toString()).splitToArray(',');
    for (i = 0; i < elementos.length; i++) {
        if (elementos[i] == obj) {
            return i;
        }
    }
    return -1;
}

function borrarNumeroArray(arr, num) {
    var i = 0;
    var elementos = (arr.toString()).splitToArray(',');

    while (elementos[i] != num && elementos[i] < num) {
        i++;
    }

    if (elementos[i] == num) {
        elementos.splice(i, 1);
    }

    return elementos.toString();
}

function agregarNumeroArray(arr, num) {
    var i = 0;
    if (arr == null || arr == '') {
        return num.toString();
    }

    var elementos = (arr.toString()).splitToArray(',');

    while (elementos[i] < num) {
        i++;
    }

    if (i == elementos.length) {
        elementos.splice(i + 1, 0, num);
    }
    else if (elementos[i] > num) {
        elementos.splice(i, 0, num);
    }

    return elementos.toString();
}