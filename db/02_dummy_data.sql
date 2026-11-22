INSERT INTO Users (name, surname, email, password_hash, address, phone)
VALUES
('Demo', 'One', 'demo1@example.com', '$2a$11$1SrGIszxtPbCIYaLY5NBvuqq4Lk/b8alRAW7ude4zO5Dy.XjANUg2', 'Example address 1', '000000001'),
('Demo', 'Two', 'demo2@example.com', '$2a$11$1SrGIszxtPbCIYaLY5NBvuqq4Lk/b8alRAW7ude4zO5Dy.XjANUg2', 'Example address 2', '000000002');

INSERT INTO Categories (name, description)
VALUES
('Kostiumy', 'Stroje demonstracyjne dla artystów cyrkowych'),
('Rekwizyty', 'Sprzęt demonstracyjny do występów cyrkowych'),
('Makijaż', 'Kosmetyki demonstracyjne do charakteryzacji'),
('Dekoracje', 'Elementy demonstracyjnej scenografii'),
('Akcesoria', 'Dodatki demonstracyjne dla artystów');

INSERT INTO Products (name, description, price, category_id, stock)
VALUES
('Kostium Klauna', 'Kolorowy kostium demonstracyjny', 299.00, 1, 15),
('Piłki żonglerskie', 'Zestaw piłek demonstracyjnych', 85.00, 2, 30),
('Strój Trapezisty', 'Elastyczny kostium demonstracyjny', 450.00, 1, 8),
('Obręcz żonglerska', 'Zestaw obręczy demonstracyjnych', 120.00, 2, 25),
('Farby do twarzy', 'Paleta kolorów demonstracyjnych', 65.00, 3, 40),
('Sztuczne kwiaty', 'Bukiet do prezentacji', 35.00, 2, 60),
('Cylinder', 'Czarny cylinder demonstracyjny', 180.00, 5, 12),
('Nos klauna', 'Gumowy nos demonstracyjny', 15.00, 5, 100),
('Lina', 'Lina demonstracyjna', 320.00, 2, 5),
('Krawat świetlny', 'Akcesorium demonstracyjne', 95.00, 5, 20);

INSERT INTO Inventory (product_id, quantity)
SELECT id, stock FROM Products;
