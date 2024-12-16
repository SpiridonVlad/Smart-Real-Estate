import pandas as pd

def validate_data(file_path):
    # Load the CSV file
    df = pd.read_csv(file_path)
    initial_row_count = len(df)

    # List of fields to transform to numeric
    fields_to_transform = [
        'sq_mt_built', 'n_rooms', 'buy_price', 'built_year', 'floor'
    ]

    # List of fields to check for missing values but not transform to numeric
    fields_to_check = [
        'title', 'subtitle', 'has_parking'
    ]

    # Combine both lists to check for missing values
    fields_to_check_all = fields_to_transform + fields_to_check

    # Print the number of missing values for each specified column
    for field in fields_to_check_all:
        if field in df.columns:
            missing_count = df[field].isna().sum()
            print(f"Column '{field}' has {missing_count} missing values")


    # Transform non-numeric fields to numeric
    for field in fields_to_transform:
        if field in df.columns:
            df[field] = pd.to_numeric(df[field], errors='coerce')
            df.dropna(subset=[field], inplace=True)

    # Drop rows with missing values in the specified fields
    for field in fields_to_check:
        if field in df.columns:
            df.dropna(subset=[field], inplace=True)

    final_row_count = len(df)
    rows_deleted = initial_row_count - final_row_count

    # Print the number of rows deleted and the number of rows left
    print(f"Rows deleted: {rows_deleted}")
    print(f"Rows left: {final_row_count}")

    # Save the cleaned data to a new CSV file
    df.to_csv('cleaned_data.csv', index=False)

# Example usage
validate_data('houses_Madrid.csv')