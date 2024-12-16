import sqlite3
import psycopg2
import bcrypt
import random
import uuid
from faker import Faker
import json

class ListingAssets:
    IS_SOLD = 'isSold'
    IS_HIGHLIGHTED = 'isHighlighted'
    IS_DELETED = 'isDeleted'

class PropertyFeatureType:
    GARDEN = 'Garden'
    GARAGE = 'Garage'
    POOL = 'Pool'
    BALCONY = 'Balcony'
    ROOMS = 'Rooms'
    SURFACE = 'Surface'
    FLOOR = 'Floor'
    YEAR = 'Year'
    HEATING_UNIT = 'HeatingUnit'
    AIR_CONDITIONING = 'AirConditioning'
    ELEVATOR = 'Elevator'
    FURNISHED = 'Furnished'
    PARKING = 'Parking'
    STORAGE = 'Storage'
    BASEMENT = 'Basement'
    ATTIC = 'Attic'
    ALARM = 'Alarm'
    INTERCOM = 'Intercom'
    VIDEO_SURVEILLANCE = 'VideoSurveillance'
    FIRE_ALARM = 'FireAlarm'

class PropertyType:
    APARTMENT = 'Apartment'
    OFFICE = 'Office'
    STUDIO = 'Studio'
    COMMERCIAL_SPACE = 'CommercialSpace'
    HOUSE = 'House'
    GARAGE = 'Garage'

class UserType:
    LEGAL_ENTITY = 'LegalEntity'
    INDIVIDUAL = 'Individual'
    ADMIN = 'Admin'

def generate_users(num_users=50):
    fake = Faker()
    users = []

    for _ in range(num_users):
        user_id = str(uuid.uuid4()).upper()
        username = fake.user_name()
        email = fake.email()
        verified = random.choice([True, False])
        raw_password = fake.password(length=12)
        hashed_password = bcrypt.hashpw(raw_password.encode('utf-8'), bcrypt.gensalt())
        property_history = None
        
        user_type = random.choice(range(3))

        users.append({
            'id': user_id,
            'username': username,
            'email': email,
            'verified': verified,
            'password': hashed_password,
            'type': user_type,
            'property_history': property_history
        })

    return users

def create_sqlite_users_table(db_path):
    conn = sqlite3.connect(db_path)
    cursor = conn.cursor()

    cursor.execute('DROP TABLE IF EXISTS Users')  # Șterge tabelul existent


    cursor.execute('''
    CREATE TABLE IF NOT EXISTS Users (
        Id TEXT PRIMARY KEY,
        Username TEXT NOT NULL,
        Email TEXT NOT NULL,
        Password TEXT NOT NULL,
        Verified BOOLEAN NOT NULL,
        Rating INTEGER,
        Type TEXT NOT NULL,
        PropertyHistory TEXT
    )
    ''')

    return conn, cursor

def populate_sqlite_users(db_path):
    conn, cursor = create_sqlite_users_table(db_path)

    users = generate_users()

    for user in users:
        cursor.execute('''
        INSERT INTO Users (Id, Username, Email, Password, Verified, Rating, Type, PropertyHistory)
        VALUES (?, ?, ?, ?, ?, ?, ?, ?)
        ''', (
            user['id'],
            user['username'],
            user['email'],
            user['password'].decode('utf-8'),
            user['verified'],
            random.randint(0, 5),
            user['type'],
            user['property_history']
        ))

    conn.commit()
    conn.close()

    print(f"Successfully populated SQLite database with {len(users)} users.")

    return users

def generate_addresses(num_addresses=50):
    fake = Faker()
    addresses = []

    for _ in range(num_addresses):
        address = {
            'id': str(uuid.uuid4()),
            'street': fake.street_address(),
            'city': fake.city(),
            'state': fake.state(),
            'postal_code': fake.postcode(),
            'country': fake.country(),
            'additional_info': fake.secondary_address() if random.random() < 0.3 else None
        }
        addresses.append(address)

    return addresses

def generate_properties(users, addresses, num_properties=50):
    properties = []

    for _ in range(num_properties):
        property_features = {
                feature: random.randint(0, 30)
                for feature in vars(PropertyFeatureType).values()
                if isinstance(feature, str) and feature.startswith('__') is False
        }
        property_types = {
                value: index for index, value in enumerate([
                        value for value in dir(PropertyType)
                        if not value.startswith('__')
                ])
        }
        
        property_obj = {
            'id': str(uuid.uuid4()),
            'user_id': random.choice(users)['id'],
            'address_id': random.choice(addresses)['id'],
            'image_id': str(uuid.uuid4()),
            'type': property_types[random.choice(list(property_types.keys()))],
            'features': json.dumps(property_features)
        }
        properties.append(property_obj)

    return properties

def generate_listings(users, properties, num_listings=50):
    listings = []

    for _ in range(num_listings):
        property_choice = random.choice(properties)
        available_users = [u for u in users if u['id'] != property_choice['user_id']]
        
        listing_features = {
                feature: random.choice([1, 0])
                for feature in vars(ListingAssets).values()
                if isinstance(feature, str) and feature.startswith('__') is False
        }
        print(listing_features)
        listing = {
            'id': str(uuid.uuid4()),
            'property_id': property_choice['id'],
            'user_id': random.choice(available_users)['id'],
            'price': random.randint(10, 1000),
            'publication_date': str(Faker().date_this_year()),
            'description': Faker().text(max_nb_chars=1000) if random.random() < 0.7 else None,
            'features': json.dumps(listing_features)
        }
        listings.append(listing)

    return listings

def populate_postgres_database(connection_string, users):
    conn = psycopg2.connect(connection_string)
    cursor = conn.cursor()

    cursor.execute('DROP TABLE IF EXISTS Users')
    cursor.execute('DROP TABLE IF EXISTS Addresses CASCADE')
    cursor.execute('DROP TABLE IF EXISTS Properties CASCADE')
    cursor.execute('DROP TABLE IF EXISTS Listings CASCADE')
    
    cursor.execute('''
    CREATE TABLE IF NOT EXISTS Addresses (
        "Id" UUID PRIMARY KEY,
        "Street" TEXT NOT NULL,
        "City" TEXT NOT NULL,
        "State" TEXT NOT NULL,
        "PostalCode" TEXT NOT NULL,
        "Country" TEXT NOT NULL,
        "AdditionalInfo" TEXT
    )
    ''')
    
    cursor.execute('''
    CREATE TABLE IF NOT EXISTS Properties (
        "Id" UUID PRIMARY KEY,
        "UserId" UUID NOT NULL,
        "AddressId" UUID NOT NULL,
        "ImageId" UUID NOT NULL,
        "Type" TEXT NOT NULL,
        "features" JSONB
    )
    ''')
    
    cursor.execute('''
    CREATE TABLE IF NOT EXISTS Listings (
        "Id" UUID PRIMARY KEY,
        "PropertyId" UUID NOT NULL,
        "UserId" UUID NOT NULL,
        "Price" INTEGER NOT NULL,
        "PublicationDate" DATE NOT NULL,
        "Description" TEXT,
        "features" JSONB
    )
    ''')

    addresses = generate_addresses(50)
    properties = generate_properties(users, addresses, 50)
    listings = generate_listings(users, properties, 50)
    
    for address in addresses:
        cursor.execute('''
        INSERT INTO Addresses ("Id", "Street", "City", "State", "PostalCode", "Country", "AdditionalInfo")
        VALUES (%s, %s, %s, %s, %s, %s, %s)
        ''', (
                address['id'], address['street'], address['city'], address['state'],
                address['postal_code'], address['country'], address['additional_info']
        ))
    
    for property_obj in properties:
        cursor.execute('''
        INSERT INTO Properties ("Id", "UserId", "AddressId", "ImageId", "Type", "features")
        VALUES (%s, %s, %s, %s, %s, %s)
        ''', (
                property_obj['id'], property_obj['user_id'], property_obj['address_id'],
                property_obj['image_id'], property_obj['type'], property_obj['features']
        ))
    
    for listing in listings:
        cursor.execute('''
        INSERT INTO Listings ("Id", "PropertyId", "UserId", "Price", "PublicationDate", "Description", "features")
        VALUES (%s, %s, %s, %s, %s, %s, %s)
        ''', (
                listing['id'], listing['property_id'], listing['user_id'],
                listing['price'], listing['publication_date'], listing['description'],
                listing['features']
        ))

    conn.commit()
    conn.close()

    print(f"Successfully populated PostgreSQL database:")
    print(f"- {len(addresses)} addresses")
    print(f"- {len(properties)} properties")
    print(f"- {len(listings)} listings")

if __name__ == '__main__':
    SQLITE_DB_PATH = 'users.db'
    POSTGRES_CONNECTION_STRING = "postgresql://postgres:123456@localhost:5432/RealEstateManagement"

    # Populate SQLite Users Database
    users = populate_sqlite_users(SQLITE_DB_PATH)

    # Populate PostgreSQL Database with other tables
    populate_postgres_database(POSTGRES_CONNECTION_STRING, users)
