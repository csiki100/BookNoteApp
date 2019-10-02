
/**
 * @description Interface that represents a Chapter Entity
 */
export interface Chapter {
  /**
   * @param id the Id of the Chapter
   */
  id: number;

  /**
   * @param id the Id of the Book
   */
  bookId: number;

  /**
   * @param id the Id of the User
   */
  userId: number;

  /**
   * @param id the Name of the Chapter
   */
  chapterName: string;

  /**
   * @param id the Content of the Chapter
   */
  content: string;
}
