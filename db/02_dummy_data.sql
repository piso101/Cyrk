INSERT INTO Users (name, surname, email, password_hash, address, phone) VALUES
('Jan', 'Kowalski', 'jan.kowalski@example.com', '$2a$11$1SrGIszxtPbCIYaLY5NBvuqq4Lk/b8alRAW7ude4zO5Dy.XjANUg2', 'ul. Przykładowa 1, Warszawa', '123456789'),
('Anna', 'Nowak', 'anna.nowak@example.com', '$2a$11$1SrGIszxtPbCIYaLY5NBvuqq4Lk/b8alRAW7ude4zO5Dy.XjANUg2', 'ul. Fikcyjna 2, Kraków', '987654321');

INSERT INTO Categories (name, description) VALUES
('Elektronika', 'Urządzenia elektroniczne'),
('Odzież', 'Ubrania i akcesoria');

INSERT INTO Products (name, description, price, category_id, stock) VALUES
('Laptop', 'Laptop gamingowy', 4500.00, 1, 10),
('Koszulka', 'Koszulka bawełniana', 50.00, 2, 50);

INSERT INTO Orders (user_id, total_price, status) VALUES
(1, 4550.00, 'Pending'),
(2, 50.00, 'Shipped');

INSERT INTO Order_Items (order_id, product_id, quantity, price) VALUES
(1, 1, 1, 4500.00),
(2, 2, 1, 50.00);

INSERT INTO Payments (order_id, payment_method, status, transaction_id) VALUES
(1, 'Credit Card', 'Completed', 'TXN123456'),
(2, 'PayPal', 'Pending', 'TXN987654');

INSERT INTO Inventory (product_id, quantity) VALUES
(1, 10),
(2, 50);

INSERT INTO Shipping (order_id, tracking_number, carrier, status, estimated_delivery, address) VALUES
(1, 'TRACK123', 'DHL', 'In Transit', '2025-03-15', 'ul. Przykładowa 1, Warszawa'),
(2, 'TRACK456', 'UPS', 'Delivered', '2025-03-12', 'ul. Fikcyjna 2, Kraków');

INSERT INTO Shipping_Rates (carrier, package_type, price) VALUES
('DHL', 'Small', 15.00),
('DHL', 'Medium', 25.00),
('DHL', 'Large', 40.00),
('UPS', 'Small', 12.00),
('UPS', 'Medium', 22.00),
('UPS', 'Large', 35.00);