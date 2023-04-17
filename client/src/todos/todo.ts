export enum TodoItemStatus {
  NotStarted = "NotStarted",
  InProgress = "InProgress",
  Completed = "Completed",
}

export class Todo {
  id: number | undefined;
  name: string = "";
  priority: number = 0;
  status: TodoItemStatus = TodoItemStatus.NotStarted;

  constructor(initializer?: any) {
    if (!initializer) return;
    if (initializer.id) this.id = initializer.id;
    if (initializer.name) this.name = initializer.name;
    if (initializer.priority) this.priority = initializer.priority;
    if (initializer.status) this.status = initializer.status;
  }

  GetTodoItemStatusString() {
    switch (this.status) {
      case TodoItemStatus.NotStarted:
        return "Not Started";
      case TodoItemStatus.InProgress:
        return "In Progress";
      case TodoItemStatus.Completed:
        return "Completed";
      default:
        return "Jsem truhlík!";
    }
  }

  ConvertStatusToNumber(todoStatus: TodoItemStatus) {
    switch (todoStatus) {
      case TodoItemStatus.NotStarted:
        return 0;
      case TodoItemStatus.InProgress:
        return 1;
      case TodoItemStatus.Completed:
        return 2;
    }
  }

  /** Explicit to JSON conversion required for proper handling of the enum field value. */
  toJSON(): {
    name: string;
    id: number | undefined;
    priority: number;
    status: number;
  } {
    return {
      name: this.name,
      id: this.id,
      priority: this.priority,
      status: this.ConvertStatusToNumber(this.status),
    };
  }
}
