package com.example.cyrkapi.services;

import android.content.Context;
import android.content.SharedPreferences;
import android.util.Log;

import com.example.cyrkapi.models.*;
import com.example.cyrkapi.dtos.*;
import com.example.cyrkapi.utils.CustomDateDeserializer;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.concurrent.TimeUnit;

import okhttp3.Interceptor;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.ResponseBody;
import okhttp3.Response;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;
import retrofit2.http.*;

public class ApiService {
    private static final String BASE_URL = "http://10.0.2.2:8080/"; // Localhost dla emulatora Android
    private static final String PREFS_NAME = "cyrk_api_prefs";
    private static final String TOKEN_KEY = "auth_token";

    private ApiInterface apiInterface;
    private Context context;
    private SharedPreferences sharedPreferences;
    private static ApiService instance;

    private ApiService(Context context) {
        this.context = context.getApplicationContext();
        this.sharedPreferences = context.getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE);

        OkHttpClient.Builder httpClient = new OkHttpClient.Builder()
                .connectTimeout(30, TimeUnit.SECONDS)
                .readTimeout(30, TimeUnit.SECONDS)
                .writeTimeout(30, TimeUnit.SECONDS);

        // Interceptor do automatycznego dodawania tokenu autoryzacji
        httpClient.addInterceptor(new Interceptor() {
            @Override
            public Response intercept(Chain chain) throws IOException {
                Request original = chain.request();
                String token = getAuthToken();

                if (token != null && !token.isEmpty()) {
                    Request.Builder requestBuilder = original.newBuilder()
                            .header("Authorization", token) // Bez "Bearer " - wysyłamy token bezpośrednio
                            .header("accept", "*/*");
                    Request request = requestBuilder.build();
                    return chain.proceed(request);
                }
                return chain.proceed(original);
            }
        });

        Gson gson = new GsonBuilder()
                .registerTypeAdapter(Date.class, new CustomDateDeserializer())
                .setLenient()
                .create();

        Retrofit retrofit = new Retrofit.Builder()
                .baseUrl(BASE_URL)
                .addConverterFactory(GsonConverterFactory.create(gson))
                .client(httpClient.build())
                .build();

        apiInterface = retrofit.create(ApiInterface.class);
    }

    public static synchronized ApiService getInstance(Context context) {
        if (instance == null) {
            instance = new ApiService(context);
        }
        return instance;
    }

    // Metody zarządzania tokenem
    public void saveAuthToken(String token) {
        sharedPreferences.edit().putString(TOKEN_KEY, token).apply();
    }

    public String getAuthToken() {
        return sharedPreferences.getString(TOKEN_KEY, null);
    }

    public void clearAuthToken() {
        sharedPreferences.edit().remove(TOKEN_KEY).apply();
    }

    public boolean isLoggedIn() {
        return getAuthToken() != null && !getAuthToken().isEmpty();
    }

    // Metody API

    // Auth Controller
    public void login(UserLoginDto loginDto, ApiCallback<AuthResultDto> callback) {
        apiInterface.login(loginDto).enqueue(new CallbackWrapper<>(callback));
    }

    public void register(UserRegisterDto registerDto, ApiCallback<String> callback) {
        apiInterface.register(registerDto).enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, retrofit2.Response<ResponseBody> response) {
                if (response.isSuccessful()) {
                    callback.onSuccess("Rejestracja pomyślna");
                } else {
                    callback.onError("Błąd rejestracji: " + response.code());
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callback.onError("Network Error: " + t.getMessage());
            }
        });
    }

    public void getCurrentUser(ApiCallback<UserMeDto> callback) {
        apiInterface.getCurrentUser().enqueue(new CallbackWrapper<>(callback));
    }
    public void getCurrentUserDetails(ApiCallback<User> callback) {
        getCurrentUser(new ApiCallback<UserMeDto>() {
            @Override
            public void onSuccess(UserMeDto userMeDto) {
                int userId = Integer.parseInt(userMeDto.id);
                getUser(userId, new ApiCallback<UserReadDto>() {
                    @Override
                    public void onSuccess(UserReadDto userReadDto) {
                        // Konwertuj UserReadDto na User
                        User user = new User();
                        user.id = userReadDto.id;
                        user.name = userReadDto.name;
                        user.surname = userReadDto.surname;
                        user.email = userReadDto.email;
                        user.address = userReadDto.address;
                        user.phone = userReadDto.phone;
                        user.created_at = userReadDto.created_at;

                        callback.onSuccess(user);
                    }

                    @Override
                    public void onError(String error) {
                        callback.onError(error);
                    }
                });
            }

            @Override
            public void onError(String error) {
                callback.onError("Błąd pobierania danych użytkownika: " + error);
            }
        });
    }

    // Category Controller
    public void getAllCategories(ApiCallback<List<Category>> callback) {
        apiInterface.getAllCategories().enqueue(new CallbackWrapper<>(callback));
    }

    public void getCategory(int id, ApiCallback<Category> callback) {
        apiInterface.getCategory(id).enqueue(new CallbackWrapper<>(callback));
    }

    public void createCategory(CategoryCreateDto categoryDto, ApiCallback<Category> callback) {
        apiInterface.createCategory(categoryDto).enqueue(new CallbackWrapper<>(callback));
    }

    public void updateCategory(int id, Category category, ApiCallback<Void> callback) {
        apiInterface.updateCategory(id, category).enqueue(new CallbackWrapper<>(callback));
    }

    public void deleteCategory(int id, ApiCallback<Void> callback) {
        apiInterface.deleteCategory(id).enqueue(new CallbackWrapper<>(callback));
    }

    // Product Controller
    public void getAllProducts(ApiCallback<List<Product>> callback) {
        apiInterface.getAllProducts().enqueue(new CallbackWrapper<>(callback));
    }

    public void getProduct(int id, ApiCallback<Product> callback) {
        apiInterface.getProduct(id).enqueue(new CallbackWrapper<>(callback));
    }

    public void getProductsByCategory(int categoryId, ApiCallback<List<Product>> callback) {
        apiInterface.getProductsByCategory(categoryId).enqueue(new CallbackWrapper<>(callback));
    }

    public void searchProducts(String searchTerm, ApiCallback<List<Product>> callback) {
        apiInterface.searchProducts(searchTerm).enqueue(new CallbackWrapper<>(callback));
    }

    public void createProduct(ProductCreateDto productDto, ApiCallback<Product> callback) {
        apiInterface.createProduct(productDto).enqueue(new CallbackWrapper<>(callback));
    }

    public void updateProduct(int id, ProductUpdateDto productDto, ApiCallback<Void> callback) {
        apiInterface.updateProduct(id, productDto).enqueue(new CallbackWrapper<>(callback));
    }

    public void updateProductStock(int id, int quantity, ApiCallback<Void> callback) {
        apiInterface.updateProductStock(id, quantity).enqueue(new CallbackWrapper<>(callback));
    }

    public void deleteProduct(int id, ApiCallback<Void> callback) {
        apiInterface.deleteProduct(id).enqueue(new CallbackWrapper<>(callback));
    }

    // Order Controller

    // W ApiService.java dodaj te metody:

    /**
     * Tworzy zamówienie i automatycznie odejmuje stan magazynowy
     */
    public void createOrderWithStockUpdate(OrderCreateDto orderDto, List<CartItem> cartItems, ApiCallback<Order> callback) {
        // Najpierw utwórz zamówienie
        createOrder(orderDto, new ApiCallback<Order>() {
            @Override
            public void onSuccess(Order result) {
                // Jeśli zamówienie się udało, odejmij stan magazynowy
                updateProductStock(cartItems, new ApiCallback<Void>() {
                    @Override
                    public void onSuccess(Void stockResult) {
                        // Wszystko się udało - zwróć zamówienie
                        callback.onSuccess(result);
                    }

                    @Override
                    public void onError(String error) {
                        // Stan się nie zaktualizował, ale zamówienie jest już utworzone
                        // Loguj błąd ale nie blokuj procesu
                        android.util.Log.w("ApiService", "Nie udało się zaktualizować stanu: " + error);
                        callback.onSuccess(result); // Zwróć zamówienie mimo błędu stanu
                    }
                });
            }

            @Override
            public void onError(String error) {
                // Zamówienie się nie udało
                callback.onError(error);
            }
        });
    }

    /**
     * Aktualizuje stan magazynowy dla listy produktów z koszyka
     */
    private void updateProductStock(List<CartItem> cartItems, ApiCallback<Void> callback) {
        if (cartItems == null || cartItems.isEmpty()) {
            callback.onSuccess(null);
            return;
        }

        // Licznik do śledzenia ukończonych aktualizacji
        final int[] completedUpdates = {0};
        final int totalUpdates = cartItems.size();
        final boolean[] hasError = {false};

        for (CartItem item : cartItems) {
            Product product = item.getProduct();
            int newStock = Math.max(0, product.stock - item.getQuantity()); // Nie pozwól na ujemny stan

            updateProductStock(product.id, newStock, new ApiCallback<Void>() {
                @Override
                public void onSuccess(Void result) {
                    completedUpdates[0]++;

                    // Zaktualizuj lokalnie obiekt produktu
                    product.stock = newStock;

                    // Sprawdź czy wszystkie aktualizacje się skończyły
                    if (completedUpdates[0] >= totalUpdates) {
                        if (!hasError[0]) {
                            callback.onSuccess(null);
                        }
                    }
                }

                @Override
                public void onError(String error) {
                    hasError[0] = true;
                    completedUpdates[0]++;

                    android.util.Log.e("ApiService", "Błąd aktualizacji stanu dla produktu " + product.id + ": " + error);

                    // Sprawdź czy wszystkie aktualizacje się skończyły
                    if (completedUpdates[0] >= totalUpdates) {
                        callback.onError("Nie udało się zaktualizować stanu niektórych produktów");
                    }
                }
            });
        }
    }

    /**
     * Pomocnicza metoda do walidacji dostępności produktów przed zamówieniem
     */
    public void validateCartAvailability(List<CartItem> cartItems, ApiCallback<Boolean> callback) {
        if (cartItems == null || cartItems.isEmpty()) {
            callback.onSuccess(true);
            return;
        }

        final int[] completedChecks = {0};
        final int totalChecks = cartItems.size();
        final boolean[] allAvailable = {true};

        for (CartItem item : cartItems) {
            // Pobierz aktualny stan produktu z serwera
            getProduct(item.getProduct().id, new ApiCallback<Product>() {
                @Override
                public void onSuccess(Product currentProduct) {
                    completedChecks[0]++;

                    // Sprawdź czy wystarczy stanu
                    if (currentProduct.stock < item.getQuantity()) {
                        allAvailable[0] = false;

                        // Opcjonalnie: zaktualizuj lokalny obiekt
                        item.getProduct().stock = currentProduct.stock;
                    }

                    // Sprawdź czy wszystkie sprawdzenia się skończyły
                    if (completedChecks[0] >= totalChecks) {
                        callback.onSuccess(allAvailable[0]);
                    }
                }

                @Override
                public void onError(String error) {
                    completedChecks[0]++;
                    allAvailable[0] = false;

                    // Sprawdź czy wszystkie sprawdzenia się skończyły
                    if (completedChecks[0] >= totalChecks) {
                        callback.onError("Nie udało się sprawdzić dostępności produktów");
                    }
                }
            });
        }
    }
    public void getAllOrders(ApiCallback<List<Order>> callback) {
        apiInterface.getAllOrders().enqueue(new CallbackWrapper<>(callback));
    }

    public void getUserOrders(ApiCallback<List<Order>> callback) {
        // Najpierw pobierz ID aktualnego użytkownika
        getCurrentUser(new ApiCallback<UserMeDto>() {
            @Override
            public void onSuccess(UserMeDto userDto) {
                // Następnie pobierz wszystkie zamówienia
                apiInterface.getAllOrders().enqueue(new CallbackWrapper<List<Order>>(new ApiCallback<List<Order>>() {
                    @Override
                    public void onSuccess(List<Order> allOrders) {
                        // Filtruj zamówienia dla aktualnego użytkownika
                        List<Order> userOrders = new ArrayList<>();
                        for (Order order : allOrders) {
                            if (order.user_id == Integer.parseInt(userDto.id)) {
                                userOrders.add(order);
                            }
                        }
                        callback.onSuccess(userOrders);
                    }

                    @Override
                    public void onError(String error) {
                        callback.onError(error);
                    }
                }));
            }

            @Override
            public void onError(String error) {
                callback.onError("Błąd pobierania danych użytkownika: " + error);
            }
        });
    }

    public void getOrder(int id, ApiCallback<Order> callback) {
        apiInterface.getOrder(id).enqueue(new CallbackWrapper<>(callback));
    }

    public void createOrder(OrderCreateDto orderDto, ApiCallback<Order> callback) {
        apiInterface.createOrder(orderDto).enqueue(new CallbackWrapper<>(callback));
    }

    public void updateOrder(int id, OrderUpdateDto orderDto, ApiCallback<Void> callback) {
        apiInterface.updateOrder(id, orderDto).enqueue(new CallbackWrapper<>(callback));
    }

    public void updateOrderStatus(int id, String status, ApiCallback<Void> callback) {
        apiInterface.updateOrderStatus(id, status).enqueue(new CallbackWrapper<>(callback));
    }

    public void deleteOrder(int id, ApiCallback<Void> callback) {
        apiInterface.deleteOrder(id).enqueue(new CallbackWrapper<>(callback));
    }

    // User Controller
    public void getAllUsers(ApiCallback<List<UserReadDto>> callback) {
        apiInterface.getAllUsers().enqueue(new CallbackWrapper<>(callback));
    }

    public void getUser(int id, ApiCallback<UserReadDto> callback) {
        apiInterface.getUser(id).enqueue(new CallbackWrapper<>(callback));
    }

    public void updateUser(int id, UserUpdateDto userDto, ApiCallback<UserReadDto> callback) {
        apiInterface.updateUser(id, userDto).enqueue(new CallbackWrapper<>(callback));
    }

    public void deleteUser(int id, ApiCallback<Void> callback) {
        apiInterface.deleteUser(id).enqueue(new CallbackWrapper<>(callback));
    }

    // Interface dla callbacków
    public interface ApiCallback<T> {
        void onSuccess(T result);
        void onError(String error);
    }

    // Wrapper dla retrofit callbacks
    private class CallbackWrapper<T> implements Callback<T> {
        private ApiCallback<T> callback;

        public CallbackWrapper(ApiCallback<T> callback) {
            this.callback = callback;
        }

        @Override
        public void onResponse(Call<T> call, retrofit2.Response<T> response) {
            if (response.isSuccessful() && response.body() != null) {
                callback.onSuccess(response.body());
            } else {
                String error = "API Error: " + response.code();
                if (response.errorBody() != null) {
                    try {
                        error += " - " + response.errorBody().string();
                    } catch (IOException e) {
                        Log.e("ApiService", "Error reading error body", e);
                    }
                }
                callback.onError(error);
            }
        }

        @Override
        public void onFailure(Call<T> call, Throwable t) {
            callback.onError("Network Error: " + t.getMessage());
        }
    }

    // Retrofit interface
    private interface ApiInterface {
        // Auth endpoints
        @POST("api/Auth/login")
        Call<AuthResultDto> login(@Body UserLoginDto loginDto);

        @POST("api/Auth/register")
        Call<ResponseBody> register(@Body UserRegisterDto registerDto);

        @GET("api/Auth/me")
        Call<UserMeDto> getCurrentUser();

        // Category endpoints
        @GET("api/Category")
        Call<List<Category>> getAllCategories();

        @GET("api/Category/{id}")
        Call<Category> getCategory(@Path("id") int id);

        @POST("api/Category")
        Call<Category> createCategory(@Body CategoryCreateDto categoryDto);

        @PUT("api/Category/{id}")
        Call<Void> updateCategory(@Path("id") int id, @Body Category category);

        @DELETE("api/Category/{id}")
        Call<Void> deleteCategory(@Path("id") int id);

        // Product endpoints
        @GET("api/Product")
        Call<List<Product>> getAllProducts();

        @GET("api/Product/{id}")
        Call<Product> getProduct(@Path("id") int id);

        @GET("api/Product/category/{categoryId}")
        Call<List<Product>> getProductsByCategory(@Path("categoryId") int categoryId);

        @GET("api/Product/search/{searchTerm}")
        Call<List<Product>> searchProducts(@Path("searchTerm") String searchTerm);

        @POST("api/Product")
        Call<Product> createProduct(@Body ProductCreateDto productDto);

        @PUT("api/Product/{id}")
        Call<Void> updateProduct(@Path("id") int id, @Body ProductUpdateDto productDto);

        @PATCH("api/Product/{id}/stock/{quantity}")
        Call<Void> updateProductStock(@Path("id") int id, @Path("quantity") int quantity);

        @DELETE("api/Product/{id}")
        Call<Void> deleteProduct(@Path("id") int id);

        // Order endpoints
        @GET("api/Orders")
        Call<List<Order>> getAllOrders();

        @GET("api/Orders/{id}")
        Call<Order> getOrder(@Path("id") int id);

        @POST("api/Orders")
        Call<Order> createOrder(@Body OrderCreateDto orderDto);

        @PUT("api/Orders/{id}")
        Call<Void> updateOrder(@Path("id") int id, @Body OrderUpdateDto orderDto);

        @PATCH("api/Orders/{id}/status")
        Call<Void> updateOrderStatus(@Path("id") int id, @Body String status);

        @DELETE("api/Orders/{id}")
        Call<Void> deleteOrder(@Path("id") int id);

        // User endpoints
        @GET("api/User")
        Call<List<UserReadDto>> getAllUsers();

        @GET("api/User/{id}")
        Call<UserReadDto> getUser(@Path("id") int id);

        @PUT("api/User/{id}")
        Call<UserReadDto> updateUser(@Path("id") int id, @Body UserUpdateDto userDto);

        @DELETE("api/User/{id}")
        Call<Void> deleteUser(@Path("id") int id);
    }
}