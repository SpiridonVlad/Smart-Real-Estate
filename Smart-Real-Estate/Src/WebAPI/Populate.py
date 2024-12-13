import sqlite3
import bcrypt
import random
import uuid
from faker import Faker


class UserType:
	REGULAR = 0
	ADMIN = 1
	MODERATOR = 2


def generate_users(num_users=100):
	fake = Faker()
	users = []
	
	for _ in range(num_users):
		user_id = str(uuid.uuid4())
		username = fake.user_name()
		email = fake.email()
		
		raw_password = fake.password(length=12)
		hashed_password = bcrypt.hashpw(raw_password.encode('utf-8'), bcrypt.gensalt())
		
		user_type = random.choice([UserType.REGULAR, UserType.ADMIN, UserType.MODERATOR])
		verified = random.choice([True, False])
		
		rating = round(random.uniform(1.0, 5.0), 2)
		
		users.append({
				'id'      : user_id,
				'username': username,
				'email'   : email,
				'password': hashed_password,
				'verified': verified,
				'rating'  : rating,
				'type'    : user_type
		})
	
	return users


def populate_database(db_path):
	conn = sqlite3.connect(db_path)
	cursor = conn.cursor()
	
	cursor.execute('''
    CREATE TABLE IF NOT EXISTS Users (
        Id TEXT PRIMARY KEY,
        Username TEXT NOT NULL,
        Email TEXT NOT NULL,
        Password TEXT NOT NULL,
        Verified INTEGER NOT NULL,
        Rating REAL NOT NULL,
        Type INTEGER NOT NULL
    )
    ''')
	
	users = generate_users()
	
	for user in users:
		cursor.execute('''
        INSERT INTO Users (Id, Username, Email, Password, Verified, Rating, Type)
        VALUES (?, ?, ?, ?, ?, ?, ?)
        ''', (
				user['id'],
				user['username'],
				user['email'],
				user['password'].decode('utf-8'),
				1 if user['verified'] else 0,
				user['rating'],
				user['type']
		))
	
	conn.commit()
	conn.close()
	
	print(f"Successfully populated database with {len(users)} users.")


def verify_user(db_path, username, password):
	conn = sqlite3.connect(db_path)
	cursor = conn.cursor()
	
	cursor.execute('SELECT Password FROM Users WHERE Username = ?', (username,))
	result = cursor.fetchone()
	
	if result:
		stored_password = result[0].encode('utf-8')
		is_valid = bcrypt.checkpw(password.encode('utf-8'), stored_password)
		conn.close()
		return is_valid
	
	conn.close()
	return False


if __name__ == '__main__':
	DB_PATH = 'users.db'
	
	populate_database(DB_PATH)