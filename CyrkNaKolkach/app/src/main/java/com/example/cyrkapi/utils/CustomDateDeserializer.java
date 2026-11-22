package com.example.cyrkapi.utils;

import com.google.gson.JsonDeserializationContext;
import com.google.gson.JsonDeserializer;
import com.google.gson.JsonElement;
import com.google.gson.JsonParseException;

import java.lang.reflect.Type;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;

public class CustomDateDeserializer implements JsonDeserializer<Date> {

    // Różne formaty dat, które może zwracać API
    private static final String[] DATE_FORMATS = {
            "yyyy-MM-dd'T'HH:mm:ss.SSSSSS",     // .NET format z mikrosekundami
            "yyyy-MM-dd'T'HH:mm:ss.SSS",       // Format z milisekundami
            "yyyy-MM-dd'T'HH:mm:ss",           // Format podstawowy
            "yyyy-MM-dd'T'HH:mm:ss'Z'",        // Format z Z
            "yyyy-MM-dd'T'HH:mm:ss.SSS'Z'",    // Format z milisekundami i Z
            "yyyy-MM-dd HH:mm:ss",             // Format bez T
            "yyyy-MM-dd"                       // Tylko data
    };

    @Override
    public Date deserialize(JsonElement json, Type typeOfT, JsonDeserializationContext context)
            throws JsonParseException {

        String dateString = json.getAsString();

        // Obsługa pustej daty lub null
        if (dateString == null || dateString.trim().isEmpty()) {
            return null;
        }

        // Obsługa domyślnej daty .NET (0001-01-01T00:00:00)
        if (dateString.startsWith("0001-01-01")) {
            return new Date(0); // Zwróć epoch (1970-01-01)
        }

        // Próbuj parsować różne formaty
        for (String format : DATE_FORMATS) {
            try {
                SimpleDateFormat sdf = new SimpleDateFormat(format, Locale.US);
                sdf.setLenient(true);
                return sdf.parse(dateString);
            } catch (ParseException e) {
                // Kontynuuj z następnym formatem
            }
        }

        // Jeśli żaden format nie zadziałał
        throw new JsonParseException("Nieprawidłowy format daty: " + dateString);
    }
}