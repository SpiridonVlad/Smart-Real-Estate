import sqlite3
import psycopg2
import bcrypt
import random
import uuid
from faker import Faker
import json
from datetime import datetime
from enum import Enum

class ListingType:
    IS_SOLD = 'IsSold'
    IS_HIGHLIGHTED = 'IsHighlighted'
    IS_DELETED = 'IsDeleted'

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

class UserStatus(Enum):
    ACTIVE = 0
    INACTIVE = 1


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
        property_history = [str(uuid.uuid4()) for _ in range(random.randint(0, 5))]
        property_waiting_list = [str(uuid.uuid4()) for _ in range(random.randint(0, 3))]
        chat_ids = [str(uuid.uuid4()) for _ in range(random.randint(1, 5))]
        
        user_type = random.choice([UserType.LEGAL_ENTITY, UserType.INDIVIDUAL, UserType.ADMIN])
        status = random.choice([UserStatus.ACTIVE, UserStatus.INACTIVE])
        
        users.append({
                'id'                   : user_id,
                'username'             : username,
                'email'                : email,
                'verified'             : verified,
                'password'             : hashed_password,
                'type'                 : user_type,
                'status'               : status.value,
                'property_history'     : json.dumps(property_history),
                'property_waiting_list': json.dumps(property_waiting_list),
                'chat_ids'             : json.dumps(chat_ids),
                'rating'               : round(random.uniform(0, 5), 1)
        })
    
    return users

def create_sqlite_users_table(db_path):
    conn = sqlite3.connect(db_path)
    cursor = conn.cursor()

    cursor.execute('''
    CREATE TABLE IF NOT EXISTS Users (
        Id TEXT PRIMARY KEY,
        Username TEXT NOT NULL,
        Email TEXT NOT NULL,
        Password TEXT NOT NULL,
        Verified BOOLEAN NOT NULL,
        Rating DECIMAL(3,1),
        Status INTEGER NOT NULL,
        Type TEXT NOT NULL,
        PropertyHistory TEXT,
        PropertyWaitingList TEXT,
        ChatIds TEXT
    )
    ''')

    return conn, cursor

def populate_sqlite_users(db_path):
    conn, cursor = create_sqlite_users_table(db_path)

    users = generate_users()
    chatid = str(uuid.uuid4())
    
    property_waiting_list = str(uuid.uuid4())
    
    for user in users:
        cursor.execute('''
        INSERT INTO Users (Id, Username, Email, Password, Verified, Rating,Status,Type, PropertyHistory,ChatId,PropertyWaitingList)
        VALUES (?, ?, ?, ?, ?, ?, ?, ?,?,?,?)
        ''', (
            user['id'],
            user['username'],
            user['email'],
            user['password'].decode('utf-8'),
            user['verified'],
            random.randint(0, 5),
            random.randint(0, 1),
            user['type'],
            user['property_history'],
            chatid,
            property_waiting_list
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
    fake = Faker()
    properties = []
    
    for _ in range(num_properties):
        property_features = {
                feature: random.randint(0, 30)
                for feature in vars(PropertyFeatureType).values()
                if isinstance(feature, str) and not feature.startswith('__')
        }
        
        image_ids = [str(uuid.uuid4()) for _ in range(random.randint(1, 5))]
        
        property_obj = {
                'id'        : str(uuid.uuid4()),
                'title'     : fake.catch_phrase(),
                'user_id'   : random.choice(users)['id'],
                'address_id': random.choice(addresses)['id'],
                'image_ids' : json.dumps(image_ids),
                'type'      : random.choice([getattr(PropertyType, attr) for attr in dir(PropertyType)
                                             if not attr.startswith('__')]),
                'features'  : json.dumps(property_features)
        }
        properties.append(property_obj)
    
    return properties


def generate_listings(users, properties, num_listings=50):
    fake = Faker()
    listings = []
    
    for _ in range(num_listings):
        property_choice = random.choice(properties)
        available_users = [u for u in users if u['id'] != property_choice['user_id']]
        
        listing_features = {
                feature: random.choice([0, 1])
                for feature in vars(ListingType).values()
                if isinstance(feature, str) and not feature.startswith('__')
        }
        
        user_waiting_list = [str(uuid.uuid4()) for _ in range(random.randint(0, 5))]
        
        listing = {
                'id'               : str(uuid.uuid4()),
                'property_id'      : property_choice['id'],
                'user_id'          : random.choice(available_users)['id'],
                'price'            : random.randint(10000, 1000000),
                'publication_date' : datetime.now().strftime('%Y-%m-%d'),
                'description'      : fake.text(max_nb_chars=1000) if random.random() < 0.7 else None,
                'features'         : json.dumps(listing_features),
                'user_waiting_list': json.dumps(user_waiting_list)
        }
        listings.append(listing)
    
    return listings


def populate_postgres_database(connection_string, users):
    conn = psycopg2.connect(connection_string)
    cursor = conn.cursor()
    
    # Drop existing tables
    cursor.execute('DROP TABLE IF EXISTS Users')
    cursor.execute('DROP TABLE IF EXISTS Addresses CASCADE')
    cursor.execute('DROP TABLE IF EXISTS Properties CASCADE')
    cursor.execute('DROP TABLE IF EXISTS Listings CASCADE')
    
    # Create Addresses table
    cursor.execute('''
    CREATE TABLE IF NOT EXISTS Properties (
        "Id" UUID PRIMARY KEY,
        "Title" TEXT NOT NULL,
        "UserId" UUID NOT NULL,
        "AddressId" UUID NOT NULL,
        "ImageIds" JSONB NOT NULL,
        "Type" TEXT NOT NULL,
        "Features" JSONB
    )
    ''')
    
    # Create Listings table
    cursor.execute('''
    CREATE TABLE IF NOT EXISTS Listings (
        "Id" UUID PRIMARY KEY,
        "PropertyId" UUID NOT NULL,
        "UserId" UUID NOT NULL,
        "Price" INTEGER NOT NULL,
        "PublicationDate" DATE NOT NULL,
        "Description" TEXT,
        "Features" JSONB,
        "UserWaitingList" JSONB
    )
    ''')
    
    addresses = generate_addresses(50)
    properties = generate_properties(users, addresses, 50)
    listings = generate_listings(users, properties, 50)
    
    # Insert data into tables
    for property_obj in properties:
        cursor.execute('''
        INSERT INTO Properties ("Id", "Title", "UserId", "AddressId", "ImageIds", "Type", "Features")
        VALUES (%s, %s, %s, %s, %s, %s, %s)
        ''', (
                property_obj['id'],
                property_obj['title'],
                property_obj['user_id'],
                property_obj['address_id'],
                property_obj['image_ids'],
                property_obj['type'],
                property_obj['features']
        ))
    
    for listing in listings:
        cursor.execute('''
        INSERT INTO Listings ("Id", "PropertyId", "UserId", "Price", "PublicationDate",
                            "Description", "Features", "UserWaitingList")
        VALUES (%s, %s, %s, %s, %s, %s, %s, %s)
        ''', (
                listing['id'],
                listing['property_id'],
                listing['user_id'],
                listing['price'],
                listing['publication_date'],
                listing['description'],
                listing['features'],
                listing['user_waiting_list']
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
