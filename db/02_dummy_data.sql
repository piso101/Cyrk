INSERT INTO Users (name, surname, email, password_hash, address, phone) 
VALUES 
('Marcel', 'Żongler', 'marcel.zongler@cyrk.pl', '$2a$11$1SrGIszxtPbCIYaLY5NBvuqq4Lk/b8alRAW7ude4zO5Dy.XjANUg2', 'ul. Cyrkowa 15, Warszawa', '123456789'),
('Klaudia', 'Balonowa', 'klaudia.balonowa@cyrk.pl', '$2a$11$1SrGIszxtPbCIYaLY5NBvuqq4Lk/b8alRAW7ude4zO5Dy.XjANUg2', 'ul. Artystów 8, Kraków', '987654321'),
('Antoni', 'Linoskoczek', 'antoni.linoskoczek@cyrk.pl', '$2a$11$1SrGIszxtPbCIYaLY5NBvuqq4Lk/b8alRAW7ude4zO5Dy.XjANUg2', 'al. Akrobatów 23, Gdańsk', '555123789'),
('Magda', 'Iluzjonistka', 'magda.iluzjonistka@cyrk.pl', '$2a$11$1SrGIszxtPbCIYaLY5NBvuqq4Lk/b8alRAW7ude4zO5Dy.XjANUg2', 'ul. Magiczna 7, Wrocław', '444987654'),
('Paweł', 'Treser', 'pawel.treser@cyrk.pl', '$2a$11$1SrGIszxtPbCIYaLY5NBvuqq4Lk/b8alRAW7ude4zO5Dy.XjANUg2', 'ul. Zwierzęca 12, Poznań', '333555777'),
('Ola', 'Trapezistka', 'ola.trapezistka@cyrk.pl', '$2a$11$1SrGIszxtPbCIYaLY5NBvuqq4Lk/b8alRAW7ude4zO5Dy.XjANUg2', 'ul. Wysokościowa 45, Łódź', '666222444'),
('Tomek', 'Mimowładca', 'tomek.mimowladca@cyrk.pl', '$2a$11$1SrGIszxtPbCIYaLY5NBvuqq4Lk/b8alRAW7ude4zO5Dy.XjANUg2', 'pl. Ciszy 3, Katowice', '777888999');

INSERT INTO Categories (name, description) 
VALUES 
('Kostiumy', 'Stroje dla artystów cyrkowych'),
('Rekwizyty', 'Sprzęt do występów cyrkowych'),
('Makijaż', 'Kosmetyki i farby do charakteryzacji'),
('Dekoracje', 'Elementy scenograficzne'),
('Akcesoria', 'Dodatki dla artystów');

INSERT INTO Products (name, description, price, category_id, stock) 
VALUES 
('Kostium Klauna Klasyczny', 'Kolorowy strój z pomponikami i dużymi guzikami', 299.00, 1, 15),
('Piłki żonglerskie', 'Zestaw 5 piłek do żonglowania różne kolory', 85.00, 2, 30),
('Strój Trapezisty', 'Elastyczny kostium z cekinami', 450.00, 1, 8),
('Obrączki do żonglowania', 'Zestaw 3 metalowych obrączek', 120.00, 2, 25),
('Farby do twarzy', 'Paleta 12 kolorów bezpiecznych dla skóry', 65.00, 3, 40),
('Sztuczne kwiaty', 'Bukiet kolorowych kwiatów do tricków', 35.00, 2, 60),
('Cylinder magika', 'Czarny cylinder z ukrytą kieszenią', 180.00, 5, 12),
('Nos klauna czerwony', 'Gumowy nos do charakteryzacji', 15.00, 5, 100),
('Lina do linoskoku', 'Profesjonalna lina 10 metrów', 320.00, 2, 5),
('Krawat motyl świecący', 'LED krawat do występów nocnych', 95.00, 5, 20),
('Parasolka kolorowa', 'Mała parasolka do tricków', 45.00, 2, 35),
('Buty klauna', 'Wielkie żółte buty rozmiar uniwersalny', 125.00, 1, 18);

INSERT INTO Orders (user_id, total_price, status) 
VALUES 
(1, 384.00, 'Pending'),
(2, 85.00, 'Shipped'),
(3, 515.00, 'Completed'),
(4, 245.00, 'Processing'),
(1, 160.00, 'Shipped'),
(5, 690.00, 'Pending'),
(6, 320.00, 'Completed'),
(7, 95.00, 'Processing'),
(2, 140.00, 'Shipped'),
(3, 75.00, 'Completed');

INSERT INTO Order_Items (order_id, product_id, quantity, price) 
VALUES 
(1, 1, 1, 299.00),
(2, 2, 1, 85.00),
(3, 3, 1, 450.00),
(3, 5, 1, 65.00),
(4, 7, 1, 180.00),
(4, 5, 1, 65.00),
(5, 8, 2, 15.00),
(5, 10, 1, 95.00),
(5, 11, 1, 45.00),
(6, 9, 1, 320.00),
(6, 12, 1, 125.00),
(6, 4, 1, 120.00),
(7, 9, 2, 320.00),
(7, 6, 1, 35.00),
(7, 10, 1, 95.00),
(8, 10, 1, 95.00),
(9, 12, 1, 125.00),
(9, 8, 1, 15.00),
(10, 6, 1, 35.00),
(10, 11, 1, 45.00);

INSERT INTO Payments (order_id, payment_method, status, transaction_id) 
VALUES 
(1, 'Credit Card', 'Completed', 'CYR123456'),
(2, 'PayPal', 'Pending', 'CYR987654'),
(3, 'BLIK', 'Completed', 'CYR445566'),
(4, 'Credit Card', 'Processing', 'CYR778899'),
(5, 'Przelewy24', 'Completed', 'CYR112233'),
(6, 'PayPal', 'Completed', 'CYR334455'),
(7, 'Credit Card', 'Completed', 'CYR556677'),
(8, 'BLIK', 'Processing', 'CYR889900'),
(9, 'Przelewy24', 'Completed', 'CYR667788'),
(10, 'Credit Card', 'Completed', 'CYR998877');

INSERT INTO Inventory (product_id, quantity) 
VALUES 
(1, 15),
(2, 30),
(3, 8),
(4, 25),
(5, 40),
(6, 60),
(7, 12),
(8, 100),
(9, 5),
(10, 20),
(11, 35),
(12, 18);

INSERT INTO Shipping (order_id, tracking_number, carrier, status, estimated_delivery, address) 
VALUES 
(1, 'CYRK123', 'DHL', 'In Transit', '2025-06-15', 'ul. Cyrkowa 15, Warszawa'),
(2, 'CYRK456', 'UPS', 'Delivered', '2025-06-12', 'ul. Artystów 8, Kraków'),
(3, 'CYRK789', 'InPost', 'Delivered', '2025-06-10', 'al. Akrobatów 23, Gdańsk'),
(4, 'CYRK321', 'DPD', 'Processing', '2025-06-16', 'ul. Magiczna 7, Wrocław'),
(5, 'CYRK654', 'UPS', 'In Transit', '2025-06-14', 'ul. Cyrkowa 15, Warszawa'),
(6, 'CYRK987', 'DHL', 'Delivered', '2025-06-11', 'ul. Zwierzęca 12, Poznań'),
(7, 'CYRK147', 'InPost', 'Delivered', '2025-06-13', 'ul. Wysokościowa 45, Łódź'),
(8, 'CYRK258', 'DPD', 'Processing', '2025-06-17', 'pl. Ciszy 3, Katowice'),
(9, 'CYRK369', 'UPS', 'In Transit', '2025-06-15', 'ul. Artystów 8, Kraków'),
(10, 'CYRK741', 'DHL', 'Delivered', '2025-06-12', 'al. Akrobatów 23, Gdańsk');

INSERT INTO Shipping_Rates (carrier, package_type, price) 
VALUES 
('DHL', 'Small', 15.00),
('DHL', 'Medium', 25.00),
('DHL', 'Large', 40.00),
('UPS', 'Small', 12.00),
('UPS', 'Medium', 22.00),
('UPS', 'Large', 35.00),
('InPost', 'Small', 10.00),
('InPost', 'Medium', 18.00),
('InPost', 'Large', 28.00),
('DPD', 'Small', 14.00),
('DPD', 'Medium', 24.00),
('DPD', 'Large', 38.00);
