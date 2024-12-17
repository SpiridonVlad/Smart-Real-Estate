import pandas as pd

def validate_data(file_path):
    df = pd.read_csv(file_path)
    initial_row_count = len(df)

    fields_to_transform = [
        'sq_mt_built', 'n_rooms', 'buy_price', 'built_year', 'floor'
    ]

    fields_to_check = [
        'title', 'subtitle', 'has_parking'
    ]

    fields_to_check_all = fields_to_transform + fields_to_check

    for field in fields_to_check_all:
        if field in df.columns:
            missing_count = df[field].isna().sum()
            print(f"Column '{field}' has {missing_count} missing values")


    for field in fields_to_transform:
        if field in df.columns:
            df[field] = pd.to_numeric(df[field], errors='coerce')
            df.dropna(subset=[field], inplace=True)

    for field in fields_to_check:
        if field in df.columns:
            df.dropna(subset=[field], inplace=True)

    final_row_count = len(df)
    rows_deleted = initial_row_count - final_row_count

    print(f"Rows deleted: {rows_deleted}")
    print(f"Rows left: {final_row_count}")

    df.to_csv('cleaned_data.csv', index=False)

# Example usage
validate_data('houses_Madrid.csv')