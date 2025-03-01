export class PageParams{
    itemsPerPage: number = 0;
    selectedPage: number = 0;
    itemsCount: number = 0;

    constructor(itemsCount: number, selectedPage: number, itemsPerPage: number){
        this.itemsCount = itemsCount;
        this.selectedPage = selectedPage;
        this.itemsPerPage = itemsPerPage;
    }
}