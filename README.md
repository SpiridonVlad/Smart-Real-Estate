# Smart Real Estate Management System

This project is a comprehensive platform for managing property listings and client inquiries. It incorporates a machine learning module to predict a properties value. The system follows a clean architecture to ensure modularity and scalability.

Key Features:

-Containerized Deployment
Built with Docker to containerize both the application and the database for consistent and reliable deployment.

-CQRS Architecture
Implements Command Query Responsibility Segregation (CQRS) to handle property listings and client inquiries separately for enhanced performance.

-MediatR Integration
Uses MediatR to manage commands and queries in a clean and decoupled manner.

-Real-Time Messaging
Powered by SignalR, providing real-time communication capabilities.

-Database Solutions
PostgreSQL for entity data storage.
SQLite for lightweight identity data storage.

-Secure Authentication and Authorization
Implements JWT (JSON Web Tokens) for robust authentication and authorization.

-Modern Frontend
Developed with Angular to deliver a responsive and intuitive user interface.

-Machine Learning Module
Includes an AI-driven module for property price predictions to enhance decision-making.
