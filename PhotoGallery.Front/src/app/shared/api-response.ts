export interface APIResponse<T>{
    data: T;
    errors: string[];
    success: boolean;
    itemsPerPage: number;
    selectedPage: number;
    itemsCount: number;
}